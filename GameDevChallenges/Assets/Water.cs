﻿using UnityEngine;
using System.Collections;

public class Water : MonoBehaviour {
	
	public struct Flux {
		public float l;
		public float r;
		public float t;
		public float b;
		
		public bool isZero() {
			return l == 0 && r == 0 && t == 0 && b == 0;
		}
		
		public static Flux operator -(Flux left, Flux right) {
			Flux ret;
			
			ret.l = left.l - right.l;
			ret.r = left.r - right.r;
			ret.b = left.b - right.b;
			ret.t = left.t - right.t;
			return ret;
			
		}
	};
	
	public IHeights terrainHeights;
	public int size = 32;
	public float radiusOfPipe = 1;
	public float accelerationDueToGravity = 9.8f; //meters per second squared
	public Material waterMaterial;
	public Material groundMaterial;
	
	TerrainData td;
	GameObject waterSurface;

	private Flux[] currentFlux;
	private float[] D1;
	private float[] D2;
	private float[,] waterHeights;
	private float[] changesInVolume;
	
	TerrainData groundTerrainData;
	GameObject groundTerrain;
	float[,] heightValues;
	private float fluxMultiplier;

	//I hardcode the time delta because the simulation will explode -> huge spikes of water everywhere, when relying
	//on Time.deltaTime. From what I can deduce, the system will get bogged down over time and cause the problem.
	private float deltaTime = 0.02f; //~ 50 frames per second
	
	private int indexOf(int x, int z) {
		return x + z*size;
	}
	
	private float totalHeightAt(int x, int z) {
		return D1[indexOf (x,z)] + heightValues[x,z];
	}
	
	private float flux(float flux, float heightDifference) {
		return Mathf.Max (0, flux + heightDifference*fluxMultiplier);
	}
	
	public float K(float height, Flux F) {
		if (F.isZero ()) {
			return 0;
		}
		
		float k = (height * 1 * 1) / ((F.l + F.r + F.t + F.b) * deltaTime);
		return Mathf.Min (1,float.IsNaN(k) ? height/float.Epsilon : k);
	}

	void Awake() {
		float areaOfPipe = Mathf.PI * Mathf.Pow (radiusOfPipe, 2.0f); //Pi * Radius^2
		fluxMultiplier = deltaTime * areaOfPipe*accelerationDueToGravity;

		D1 = new float[size * size];
		td = new TerrainData ();
		td.size = new Vector3 (10, 10, 1);
		
		waterSurface = Terrain.CreateTerrainGameObject (td);
		waterSurface.name = "Water Surface";
		waterSurface.transform.parent = this.transform;
		waterSurface.isStatic = false;
		waterSurface.GetComponent<Terrain> ().materialType = Terrain.MaterialType.Custom;
		waterSurface.GetComponent<Terrain> ().materialTemplate = waterMaterial;

		groundTerrainData = new TerrainData();
		groundTerrainData.size = new Vector3(size,size,size);
		
		heightValues = new float[size,size];

		for (int x = 0; x < size; x++) {
			for(int z =  0; z < size; z++) {

				D1[indexOf (x,z)] = 0.0f;
				heightValues[x,z] = 0.0f;

				if(x == size/2) {
					
					D1[indexOf (x,z)] = 5.0f;
					heightValues[x,z] = 5.0f;
				}
			}
		}
		
		//apply terrain
		groundTerrainData.SetHeights(0,0,heightValues);
		groundTerrain = Terrain.CreateTerrainGameObject(groundTerrainData);
		groundTerrain.name = "Ground Terrain";
		groundTerrain.transform.parent = this.transform;
		groundTerrain.GetComponent<Terrain>().terrainData = groundTerrainData;
		groundTerrain.GetComponent<TerrainCollider>().terrainData = groundTerrainData;
		groundTerrain.GetComponent<Terrain> ().materialType = Terrain.MaterialType.Custom;
		groundTerrain.GetComponent<Terrain> ().materialTemplate = groundMaterial;
		groundTerrain.GetComponent<Terrain>().Flush();
	}

	void Rainfall ()
	{
		int _x = Mathf.RoundToInt (Random.value * (size-1));
		int _z = Mathf.RoundToInt (Random.value * (size-1));

		D1 [indexOf (_x, _z)] += 0.2f;
	}
	
	void Outflow ()
	{
		for (int x = 0; x < size; x++) {
			for (int z = 0; z < size; z++) {
				
				//if the current volume of water for this grid cell is 0, then outflow to each neighbor should be 0
				//check for this, update current_flux and continue with the next iteration.
				if(D1[indexOf (x,z)] == 0) {
					currentFlux[indexOf (x,z)] = new Flux();
					continue;
				}
				
				//current height including terrain height
				float height = totalHeightAt (x,z);

				float left_height = 0;
				float right_height = 0;
				float top_height = 0;
				float bottom_height = 0;
				Flux current = currentFlux[indexOf (x,z)];
				Flux next = new Flux();

				if (x >= 1) {
					//left neighbor
					left_height = totalHeightAt (x-1, z);
					next.l = flux (current.l, height - left_height);
					
				}
				if (x <= size - 2) {
					//right neighbor eligible
					right_height = totalHeightAt (x+1, z);
					next.r = flux (current.r, height - right_height);
				}
				if (z >= 1) {
					//bottom neighbor eligible
					bottom_height = totalHeightAt (x, z-1);
					next.b = flux (current.b, height - bottom_height);
				}
				if (z <= size - 2) {
					//top neighbor eligible
					top_height = totalHeightAt (x, z+1);
					next.t = flux (current.t, height - top_height);
				}
				
				if(next.l + next.r + next.t + next.b > D1[indexOf (x,z)]) {
					
					float multiplier = K (D1[indexOf (x,z)], next);
					
					next.l *= multiplier;
					next.r *= multiplier;
					next.b *= multiplier;
					next.t *= multiplier;
				}
				currentFlux [indexOf (x, z)] = next;
			}
		}
	}
	
	void calculateChangeInVolumes ()
	{
		for (int x = 0; x < size; x++) {
			for (int z = 0; z < size; z++) {
				Flux outflow = currentFlux [indexOf (x, z)];
				float rightInflow = 0;
				float leftInflow = 0;
				float topInflow = 0;
				float bottomInflow = 0;
				if (x >= 1) {
					//left neighbor, flowing to the right
					leftInflow = currentFlux [indexOf (x - 1, z)].r;
				}
				if (x <= size - 2) {
					//right neighbor eligible, flowing to the left
					rightInflow = currentFlux [indexOf (x+1, z)].l;
				}
				if (z >= 1) {
					//bottom neighbor eligible, flowing to toward the top
					bottomInflow = currentFlux [indexOf (x, z-1)].t;
				}
				if (z <= size - 2) {
					//top neighbor eligible, flowing toward the bottom
					topInflow = currentFlux [indexOf (x, z+1)].b;
				}
				changesInVolume [indexOf (x, z)] = deltaTime * ((leftInflow + rightInflow + topInflow + bottomInflow)
				                                                - (outflow.l + outflow.r + outflow.t + outflow.b));
				D2[indexOf (x,z)] = Mathf.Max(0, D1[indexOf (x,z)] + changesInVolume[indexOf (x,z)]);
			}
		}
	}
	
	void repositionGeometry ()
	{
		for (int x = 0; x < size; x++) {
			for (int z = 0; z < size; z++) {
				
				float height = D2[indexOf (x,z)];
				if(float.IsNaN(height)) { height = 0; }

				waterHeights[z,x] = Mathf.Max (0, height/6);
			}
		}

		//apply water heights to a terrain object
		TerrainData d = new TerrainData ();
		d.size = new Vector3 (size,10,size);
		d.SetHeights (0,0,waterHeights);
		waterSurface.GetComponent<Terrain> ().terrainData = d;
		//we add it to the collider also so that we can cast rays and hit the water surface
		waterSurface.GetComponent<TerrainCollider> ().terrainData = d;
		waterSurface.GetComponent<Terrain> ().Flush ();
		
		//this line is VITAL
		//save the new height values to be used next frame in Rainfall() and Outflow()
		D1 = D2;
	}
	
	// Update is called once per frame
	void Update () {
		
		currentFlux = new Flux[size * size];
		D2 = new float[size * size];
		waterHeights = new float[size, size];
		changesInVolume = new float[size * size];
		
		if(Input.GetMouseButtonUp(1)) { //right click 
			
			Vector3 pos = new Vector3();
			
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hitInfo;
			if(Physics.Raycast(ray, out hitInfo)) {
				pos = hitInfo.point;
			}
			
			D1[indexOf (Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.z))] += 1.0f;
			
		}
		if(Input.GetMouseButtonUp(5)) { //right click
			Vector3 pos = new Vector3();
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hitInfo;
			if(Physics.Raycast(ray, out hitInfo)) {
				pos = hitInfo.point;
			}
			
			D1[indexOf (Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.z))] -= 1.0f;
			
		}
		
		Rainfall ();

		Outflow ();
		
		calculateChangeInVolumes ();
		
		repositionGeometry ();
		
	}
}
