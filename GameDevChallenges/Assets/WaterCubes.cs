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

		public static Flux operator +(Flux left, Flux right) {
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



	//I hardcode the time delta because the simulation will explode -> huge spikes of water everywhere, when relying
	//on Time.deltaTime. From what I can deduce, the system will get bogged down over time and cause the problem.
	private float deltaTime = 0.02f; //~ 50 frames per second

	private int indexOf(int x, int z) {
		return x + z*size;
	}

	private float height_of(int x, int z) {
		if (useCubes) {
			return cubes [indexOf (x, z)].transform.localScale.y + terrainCubes[indexOf (x,z)].transform.localScale.y;
		} else {
			return D1[indexOf (x,z)] + heightValues[x,z];
		}
	}

	private float flux(float flux, float heightDifference) {
		return Mathf.Max (0, flux + (deltaTime //time step
		                             * (Mathf.PI*Mathf.Pow (radiusOfPipe, 2.0f)) //Cross section of the pipe -> PI * R^2
		                             * accelerationDueToGravity //9.8 m/s^2
		                             * heightDifference/1.0f)); //height difference over length of pipe
	}

	public float K(float height, Flux F) {
		if (F.isZero ()) {
			return 0;
		}

		float k = (height * 1 * 1) / ((F.l + F.r + F.t + F.b) * deltaTime);
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
//		waterSurface.AddComponent<TerrainMouseOver> ();
//		waterSurface.GetComponent<TerrainMouseOver> ().marker = marker;
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

					float t_height = Mathf.Abs(x - size/2);// + Mathf.Abs(z - size/2);// Random.value * 3f;
//					float t_height = x/3.0f;

//					if(x == size/2) { t_height = 1.0f; }

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
					heightValues[x,z] = 0.0f;
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

				//if the current volume of water for this grid cell is 0, then outflow to each neighbor should be 0
				//check for this, update current_flux and continue with the next iteration.
				if(D1[indexOf (x,z)] == 0) {
					current_flux[indexOf (x,z)] = new Flux();
					continue;
				}

				//current height
				float height = height_of (x,z);

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

				float sum = next.l + next.r + next.t + next.b;
				Flux weights = new Flux();
				weights.l = next.l / sum;
				weights.r = next.r / sum;
				weights.b = next.b /sum;
				weights.t = next.t / sum;

				next.l = weights.l * height;
				next.r = weights.r * height;
				next.b = weights.b * height;
				next.t = weights.t * height;



//				if(next.l + next.r + next.t + next.b > height) {
//
//					float multiplier = K (height, next);
//
//					next.l *= multiplier;
//					next.r *= multiplier;
//					next.b *= multiplier;
//					next.t *= multiplier;
//				}
				current_flux [indexOf (x, z)] = next;
			}
		}
	}

	void calculateChangeInVolumes ()
	{
		for (int x = 0; x < size; x++) {
			for (int z = 0; z < size; z++) {
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
				changesInVolume [indexOf (x, z)] = deltaTime * ((l_flow_in + r_flow_in + t_flow_in + b_flow_in) - (outflow.l + outflow.r + outflow.t + outflow.b));
				D2[indexOf (x,z)] = D1[indexOf (x,z)] + changesInVolume[indexOf (x,z)];
			}
		}
	}

	void repositionGeometry ()
	{
		for (int x = 0; x < size; x++) {
			for (int z = 0; z < size; z++) {

				float height = D2[indexOf (x,z)];
				if(float.IsNaN(height)) { height = 0; }

				if(useCubes) {
					GameObject waterCube = cubes [indexOf (x, z)];
					GameObject terrainCube = terrainCubes[indexOf (x,z)];

					float t_height = terrainCube.transform.localScale.y;
					Vector3 w_position = new Vector3(x, Mathf.Max (t_height, t_height + (height/2.0f)), z);

					waterCube.transform.position = w_position;
					waterCube.transform.localScale = new Vector3(1, Mathf.Max (0,height), 1);

				}
				waterHeights[z,x] = Mathf.Max (0, height/6);
			}
		}

		TerrainData d = new TerrainData ();
		d.size = new Vector3 (size,10,size);
		d.SetHeights (0,0,waterHeights);
		
		waterSurface.GetComponent<Terrain> ().terrainData = d;
		
		//we add it to the collider also so that we can cast rays and hit the water surface
		waterSurface.GetComponent<TerrainCollider> ().terrainData = d;
		waterSurface.GetComponent<Terrain> ().Flush ();

		//this line is VITAL
		D1 = D2;
	}

	// Update is called once per frame
	void Update () {
//		Debug.Log ("TIME DELTA: " + deltaTime);
		current_flux = new Flux[size * size];
		D2 = new float[size * size];
		waterHeights = new float[size, size];
		changesInVolume = new float[size * size];

		if(Input.GetMouseButtonUp(0)) { //left click and hold
			Debug.Log ("BOOM!");
			Vector3 pos = new Vector3();


			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hitInfo;
			if(Physics.Raycast(ray, out hitInfo)) {
				pos = hitInfo.point;
			}


			D1[indexOf (Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.z))] += 0.5f;
			D1[indexOf (Mathf.RoundToInt(pos.x+1), Mathf.RoundToInt(pos.z))] += 0.5f;
			D1[indexOf (Mathf.RoundToInt(pos.x+1), Mathf.RoundToInt(pos.z+1))] += 0.5f;
			D1[indexOf (Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.z+1))] += 0.5f;


		}
		if(Input.GetMouseButtonUp(1)) { //right click and hold
			Vector3 pos = new Vector3();
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hitInfo;
			if(Physics.Raycast(ray, out hitInfo)) {
				pos = hitInfo.point;
			}



			D1[indexOf (Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.z))] -= 0.5f;
			D1[indexOf (Mathf.RoundToInt(pos.x+1), Mathf.RoundToInt(pos.z))] -= 0.5f;
			D1[indexOf (Mathf.RoundToInt(pos.x+1), Mathf.RoundToInt(pos.z+1))] -= 0.5f;
			D1[indexOf (Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.z+1))] -= 0.5f;
		}

		//Rainfall ();

		Outflow ();

		calculateChangeInVolumes ();

		repositionGeometry ();
	}
}
