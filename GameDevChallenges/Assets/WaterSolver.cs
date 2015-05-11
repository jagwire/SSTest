using UnityEngine;
using System.Collections;

public class WaterSolver : MonoBehaviour {
/*	
	struct Flux {
		public float L;
		public float R;
		public float T;
		public float B;
	}
	
	struct Cell {
		public float height;
		public Flux F;
		public float changeInVolume;
		public float K() {
			return Mathf.Min (1, (height*1*1)/((F.L + F.R + F.T + F.B)*Time.deltaTime));
		}
	};
	
	public float accelerationDueToGravity;
	public float rateOfRainFall;
	public DynamicWorld world;
	public Terrain water;
	public int XZSize;
	
	
	private float[,] waterHeights;
	SparseGrid<Cell> water_cells;
	// Use this for initialization
	void Start () {
		waterHeights = new float[XZSize,XZSize];
		water_cells = new SparseGrid<Cell> (XZSize,XZSize);
	}
	
	// Update is called once per frame
	void Update () {
		WaterMain ();
	}
	private float flux(float flux, float heightDifference) {
		return Mathf.Max (0, flux + (Time.deltaTime * Mathf.PI * accelerationDueToGravity * heightDifference/1));
	}
	
	public float[,] ToArray() {
		float[,] output = new float[XZSize, XZSize];
		
		for (int x = 0; x < XZSize; x++) {
			for(int z = 0; z< XZSize; z++) {
				output[x,z] = water_cells[x, 0, z].height;
			}
		}
		
		return output;
	}
	
	private void WaterMain() {
		//step 1: update height from rainfall: height = rate * time;
		rainfalls();
		
		//step 2: update flux for every cell in grid
		waterFlowsOutward ();
		
		//step 3: caculate change in Volume
		waterVolumeChanges ();
		
		//step 4: update Water Height
		waterChangesHeight ();
		
		//finally: set terrain values;
		water.terrainData.SetHeights (0, 0, ToArray ()); 
	}
	
	
	
	
	void rainfalls() {
		for(int x = 0; x < XZSize; x++) {
			for(int y = 0; y < XZSize; y++) {
				waterHeights[y, x] += Time.deltaTime*rateOfRainFall;
			}
		}
	}
	
	float height_delta(int x, int z, int x1, int z1) {
		//todo: replace this with water_cells or such
		return world.GetHeightAt(x, z)+water_cells[x,0,z].height - world.GetHeightAt(x1, z1) - water_cells[x1, 0, z1].height;
	}
	
	void waterChangesHeight() {
		for (int x = 0; x < XZSize; x++) {
			for(int z = 0; z < XZSize; z++) {
				Cell current = water_cells[x, 0, z];
				current.height += (current.changeInVolume/1*1);
			}
		}
	}
	
	void waterVolumeChanges() {
		for (int x = 0; x < XZSize; x++) {
			for(int z = 0; z < XZSize; z++) {
				Cell current = water_cells[x, 0, z];
				
				float r_flow_in = 0;
				float l_flow_in = 0;
				float t_flow_in = 0;
				float b_flow_in = 0;
				
				if(x >= 1) {
					//left neighbor
					l_flow_in = water_cells[x-1, 0, z].F.R;
					
				}
				
				if(x <= XZSize - 2) {
					//right neighbor eligible
					int _x = x+1;
					int _z = z;
					r_flow_in = water_cells[_x, 0, _z].F.L;
				}
				
				if(z >= 1) {
					//bottom neighbor eligible
					int _x = x;
					int _z = z-1;
					b_flow_in = water_cells[_x, 0, _z].F.T;
				}
				
				if(z <= XZSize - 2) {
					//top neighbor eligible
					int _x = x;
					int _z = z+1;
					
					t_flow_in = water_cells[_x, 0, _z].F.B;
				}
				
				
				current.changeInVolume = Time.deltaTime*((l_flow_in + r_flow_in + t_flow_in + b_flow_in)
				                                         -(current.F.L + current.F.R + current.F.T + current.F.B));
			}
		}
	}
	
	
	void waterFlowsOutward() {
		for (int x = 0; x < XZSize; x++) {
			for(int z = 0; z < XZSize; z++) {
				Cell current = water_cells[x, 0, z];
				
				//get height values of all neighbors
				if(x >= 1) {
					//left neighbor
					int _x = x-1;
					int _z = z;
					
					float delta = height_delta (x, z, _x, _z);
					current.F.L = flux (current.F.L, delta);
					
				}
				
				if(x <= 10 - 2) {
					//right neighbor eligible
					int _x = x+1;
					int _z = z;
					
					float delta = height_delta (x, z, _x, _z);
					current.F.R = flux (current.F.R, delta);
				}
				
				if(z >= 1) {
					//bottom neighbor eligible
					int _x = x;
					int _z = z-1;
					
					float delta = height_delta (x, z, _x, _z);
					current.F.B = flux (current.F.B, delta);
				}
				
				if(z <= 10 - 2) {
					//top neighbor eligible
					int _x = x;
					int _z = z+1;
					
					float delta = height_delta(x, z, _x, _z);
					current.F.T = flux (current.F.T, delta);
				}
				
				current.F.L *= current.K();
				current.F.R *= current.K();
				current.F.T *= current.K();
				current.F.B *= current.K();
			}
		}
	}
	*/
}
