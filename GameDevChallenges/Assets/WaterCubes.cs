using UnityEngine;
using System.Collections;

public class WaterCubes : MonoBehaviour {

	public struct Flux {
		public float l;
		public float r;
		public float t;
		public float b;

		public bool isZero() {
			return l == 0 && r == 0 && t == 0 && b == 0;
		}
	};

	public IHeights terrainHeights;
	public int size = 32;
	public float radiusOfPipe = 1;
	public float accelerationDueToGravity = 9.8f; //meters per second squared
	public Material waterMaterial;
	public Material groundMaterial;
	public bool useCubes = false;
	public GameObject marker;

	TerrainData td;
	GameObject waterSurface;


	private GameObject[] cubes;
	private GameObject[] terrainCubes;
	private Flux[] current_flux;
	private float[] D1;
	private float[] D2;
	private float[,] waterHeights;
	private float[] changesInVolume;


	private int indexOf(int x, int z) {
		return x + z*size;
	}

	private float height_of(int x, int z) {
		if (useCubes) {
			return cubes [indexOf (x, z)].transform.localScale.y + terrainCubes[indexOf (x,z)].transform.localScale.y;
		} else {
			return D1[indexOf (x,z)];
		}
	}

	private float flux(float flux, float heightDifference) {
		return Mathf.Max (0, flux + (Time.deltaTime * (Mathf.PI*Mathf.Pow (radiusOfPipe, 2.0f)) * accelerationDueToGravity * heightDifference/1.0f));
	}

	public float K(float height, Flux F) {
		if (F.isZero ()) {
			return 0;
		}

		float k = (height * 1 * 1) / ((F.l + F.r + F.t + F.b) * Time.deltaTime);
		return Mathf.Min (1,float.IsNaN(k) ? height/float.Epsilon : k);
	}


	void Start() {

	}

	TerrainData groundTerrainData;
	GameObject groundTerrain;
	float[,] heightValues;
	void Awake() {
		D1 = new float[size * size];
		td = new TerrainData ();
		td.size = new Vector3 (10, 10, 1);
		
		waterSurface = Terrain.CreateTerrainGameObject (td);
		waterSurface.isStatic = false;
		waterSurface.GetComponent<Terrain> ().materialType = Terrain.MaterialType.Custom;
		waterSurface.GetComponent<Terrain> ().materialTemplate = waterMaterial;
		waterSurface.AddComponent<TerrainMouseOver> ();
		waterSurface.GetComponent<TerrainMouseOver> ().marker = marker;
		if (useCubes) {
			cubes = new GameObject[size * size];
			terrainCubes = new GameObject[size * size];
		} else {
			groundTerrainData = new TerrainData();
			groundTerrainData.size = new Vector3(size,size,size);

			heightValues = new float[size,size];
		}
		for (int x = 0; x < size; x++) {
			for(int z =  0; z < size; z++) {


				D1[indexOf (x,z)] = 0.0f;
				if(useCubes) {
					Vector3 position = new Vector3(x, 0, z);


					GameObject t_cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
					terrainCubes[indexOf (x,z)] = t_cube;
					float t_height = Random.value * 3f;
//					float t_height = x/3.0f;
					t_cube.transform.position = new Vector3(x,t_height/2.0f,z);
					t_cube.transform.localScale = new Vector3(1, t_height, 1);
					t_cube.GetComponent<Renderer>().material = groundMaterial;


					GameObject w_cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
					cubes[indexOf (x,z)] = w_cube;
					w_cube.transform.position = t_cube.transform.position;
					w_cube.transform.parent = this.transform;
					w_cube.transform.localScale = new Vector3(1,0,1);

					w_cube.GetComponent<Renderer>().material = waterMaterial;
				} else {
					heightValues[x,z] = Random.value/30f;
				}
			}
		}

		if (!useCubes) {
			groundTerrainData.SetHeights(0,0,heightValues);
			groundTerrain = Terrain.CreateTerrainGameObject(groundTerrainData);
			groundTerrain.GetComponent<Terrain>().terrainData = groundTerrainData;
			groundTerrain.GetComponent<TerrainCollider>().terrainData = groundTerrainData;
			groundTerrain.GetComponent<Terrain>().Flush();

		}
	}

	public float count = 0;
	void Rainfall ()
	{
		int _x = Mathf.RoundToInt (Random.value * (size-1));
		int _z = Mathf.RoundToInt (Random.value * (size-1));

		if (useCubes) {
			Vector3 waterPosition = cubes [indexOf (_x, _z)].transform.position;
			Vector3 waterScale = cubes [indexOf (_x, _z)].transform.localScale;

			float terrainHeight = terrainCubes[indexOf (_x,_z)].transform.localScale.y;

			float height = waterScale.y + 0.2f;

//			cubes [indexOf (_x, _z)].transform.localScale = new Vector3 (1, height, 1);
//			waterPosition.y = height / 2.0f;
//			cubes [indexOf (_x, _z)].transform.position = new Vector3(_x, terrainHeight + height/2.0f , _z);
			D1[indexOf (_x,_z)] = waterScale.y + 0.2f;
		} else {
			D1 [indexOf (_x, _z)] += 0.2f;
		}
		count += 1;
	}

	void Outflow ()
	{
		for (int x = 0; x < size; x++) {
			for (int z = 0; z < size; z++) {


				//current height
				float height = 0.0f;
				if(useCubes) {
					if(cubes[indexOf (x,z)].transform.localScale.y == 0) {
						current_flux[indexOf (x,z)] = new Flux();
						continue;
					}


//					height = cubes [indexOf (x, z)].transform.localScale.y + cubes[indexOf (x,z)].transform.position.y;
//					height = terrainCubes[indexOf (x,z)].transform.localScale.y + cubes[indexOf (x,z)].transform.localScale.y;
					height = height_of (x,z);
				} else {
					height = height_of (x,z);//D1[indexOf (x,z)];
				}
				float left_height = 0;
				float right_height = 0;
				float top_height = 0;
				float bottom_height = 0;
				Flux current = current_flux[indexOf (x,z)];
				Flux next = new Flux();
				if (x >= 1) {
					//left neighbor
					int _x = x - 1;
					int _z = z;
					left_height = height_of (_x, _z);
					next.l = flux (current.l, height - left_height);
				}
				if (x <= size - 2) {
					//right neighbor eligible
					int _x = x + 1;
					int _z = z;
					right_height = height_of (_x, _z);
					next.r = flux (current.r, height - right_height);
				}
				if (z >= 1) {
					//bottom neighbor eligible
					int _x = x;
					int _z = z - 1;
					bottom_height = height_of (_x, _z);
					next.b = flux (current.b, height - bottom_height);
				}
				if (z <= size - 2) {
					//top neighbor eligible
					int _x = x;
					int _z = z + 1;
					top_height = height_of (_x, _z);
					next.t = flux (current.t, height - top_height);
				}
				float multiplier = K (height, next);

				next.l *= multiplier;
				next.r *= multiplier;
				next.b *= multiplier;
				next.t *= multiplier;
				current_flux [indexOf (x, z)] = next;
			}
		}
	}

	void calculateChangeInVolumes ()
	{
		for (int x = 0; x < size; x++) {
			for (int z = 0; z < size; z++) {
				//				Cell current = water_cells[x, 0, z];
				Flux outflow = current_flux [indexOf (x, z)];
				float r_flow_in = 0;
				float l_flow_in = 0;
				float t_flow_in = 0;
				float b_flow_in = 0;
				if (x >= 1) {
					//left neighbor
					l_flow_in = current_flux [indexOf (x - 1, z)].r;
				}
				if (x <= size - 2) {
					//right neighbor eligible
					int _x = x + 1;
					int _z = z;
					r_flow_in = current_flux [indexOf (_x, _z)].l;
				}
				if (z >= 1) {
					//bottom neighbor eligible
					int _x = x;
					int _z = z - 1;
					b_flow_in = current_flux [indexOf (_x, _z)].t;
				}
				if (z <= size - 2) {
					//top neighbor eligible
					int _x = x;
					int _z = z + 1;
					t_flow_in = current_flux [indexOf (_x, _z)].b;
				}
				changesInVolume [indexOf (x, z)] = Time.deltaTime * ((l_flow_in + r_flow_in + t_flow_in + b_flow_in) - (outflow.l + outflow.r + outflow.t + outflow.b));
			
				if(useCubes) {
					D2 [indexOf (x, z)] = cubes [indexOf (x, z)].transform.localScale.y + changesInVolume [indexOf (x, z)];
				} else {
					D2[indexOf (x,z)] = D1[indexOf (x,z)] + changesInVolume[indexOf (x,z)];
				}

			}
		}
	}

	void updateHeights ()
	{
		for (int x = 0; x < size; x++) {
			for (int z = 0; z < size; z++) {

				float height = D2[indexOf (x,z)];
				if(float.IsNaN(height)) { height = 0; }

				if(useCubes) {
					GameObject waterCube = cubes [indexOf (x, z)];
					GameObject terrainCube = terrainCubes[indexOf (x,z)];

					float t_height = terrainCube.transform.localScale.y;
					Vector3 w_position = new Vector3(x, t_height + (height/2.0f), z);

					waterCube.transform.position = w_position;
					waterCube.transform.localScale = new Vector3(1, height, 1);

					waterHeights[z,x] = height;
				} else {
					waterHeights[z,x] = height/6;
				}
			}
		}

		//this line is VITAL
		D1 = D2;
	}

	// Update is called once per frame
	void Update () {
		current_flux = new Flux[size * size];
		D2 = new float[size * size];
		waterHeights = new float[size, size];
		changesInVolume = new float[size * size];

		if(Input.GetMouseButton(0)) { //left click and hold
			Debug.Log ("BOOM!");
			Vector3 pos = marker.transform.position;
			D1[indexOf (Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.z))] += 0.5f;
			D1[indexOf (Mathf.RoundToInt(pos.x+1), Mathf.RoundToInt(pos.z))] += 0.5f;
			D1[indexOf (Mathf.RoundToInt(pos.x+1), Mathf.RoundToInt(pos.z+1))] += 0.5f;
			D1[indexOf (Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.z+1))] += 0.5f;


		}
		if(Input.GetMouseButton(1)) { //right click and hold
			Vector3 pos = marker.transform.position;
			D1[indexOf (Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.z))] -= 0.5f;
			D1[indexOf (Mathf.RoundToInt(pos.x+1), Mathf.RoundToInt(pos.z))] -= 0.5f;
			D1[indexOf (Mathf.RoundToInt(pos.x+1), Mathf.RoundToInt(pos.z+1))] -= 0.5f;
			D1[indexOf (Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.z+1))] -= 0.5f;
		}

		Rainfall ();

		Outflow ();

		calculateChangeInVolumes ();

		updateHeights ();


		TerrainData d = new TerrainData ();
		d.size = new Vector3 (size,10,size);
		d.SetHeights (0,0,waterHeights);

		waterSurface.GetComponent<Terrain> ().terrainData = d;

		//we add it to the collider also so that we can cast rays and hit the water surface
		waterSurface.GetComponent<TerrainCollider> ().terrainData = d;
		waterSurface.GetComponent<Terrain> ().Flush ();
	}
}
