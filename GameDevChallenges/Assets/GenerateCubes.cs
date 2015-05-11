using UnityEngine;
using System.Collections;

public class GenerateCubes : MonoBehaviour {

	public int size = 100; 
	public Texture2D texture;
	public Gradient gradient;
	//tweak this to make more/less hills
	float TwoPI = Mathf.PI*8;


	//tweak this to make hills more defined
	float magnitude = 1f;


	//circle trials
	float fieldWidth = ((100 - 1)*0.5f)+(99-1)*0.5f;
	Vector2 circlePosition = new Vector2 (0.5f, 0.5f);
	float circleRadius = 0.25f;

	private Color[] pixels;
	private GameObject[] cubes;


	private Color GetPixelAtIndex(int x, int y) {
		//Debug.Log ("Looking for pixel: ("+x+","+y+") -> "+(x+y*texture.width));
		return pixels[x+y*texture.width];
	}

	private int indexOf(int x, int y) {
		return x + y * size;
	}

	Color fragment(int x, int y, int z) {
		//now we effectively need our UV's
		float u = (float)x / (size - 1);
		float v = (float)z / (size - 1);

		Color pixel = GetPixelAtIndex (Mathf.RoundToInt (u * (texture.height - 1)), Mathf.RoundToInt (v * (texture.width - 1)));
		Vector3 pixl = new Vector3 (pixel.r, pixel.g, pixel.b);
		float intensity = Vector3.Dot (pixl, new Vector3 (0.2126f, 0.7152f, 0.0722f)); 

		return gradient.Evaluate (intensity);
	}
	
	void Awake() {
		cubes = new GameObject[size * size];
		pixels = texture.GetPixels ();
		//Debug.Log ("length: " + pixels.Length);
		for (int x = 0; x < size; x++) {
			for (int z = 0; z < size; z++) {
				//do something with height, for now make it 0
				float height = 0f;

				//create cube our "pixel"...maybe voxel?
				GameObject cube = cubes [indexOf (x, z)] = GameObject.CreatePrimitive (PrimitiveType.Cube);
//				cubes [indexOf (x, z)] = cube;
				Vector3 position = new Vector3 (x, height, z);
				cube.transform.position = position;
				cube.transform.localScale = new Vector3 (0.5f, 0.5f, 0.5f);

				Vector3 fieldPosition = new Vector3 (x, 0, z) / 100;
				Vector2 delta = new Vector2 (fieldPosition.x, fieldPosition.z) - circlePosition;
				float distanceToCenter = delta.magnitude; 
				float scale = Mathf.Cos (distanceToCenter * 10) * 0.5f + 0.5f;
				//cube.transform.localScale = new Vector3(1*scale, 1*scale, 1*scale);
				cube.transform.localScale = Vector3.one;



				Renderer r = cube.GetComponent<Renderer>();
				Color c = fragment (x, 0, z);
				r.material.color = c;
			}
		}

		//blur ();
	}
	private void blur() {
		for(int x = 1; x < size; x++) {
			for(int z = 1; z < size; z++) {
				Color N, NE, NW, E, SE, S, SW, W, avg_color;
				float avg_red, avg_green, avg_blue;
				if( x == 0) {

					if(z == 0) {

						//lower left corner, retrieve N, NE, and E
						N = ColorOf (NorthOf (x,z));
						NE = ColorOf (NorthEastOf (x,z));
						E = ColorOf (EastOf (x,z));
						avg_red = N.r + NE.r + E.r;
						avg_red /=3;
						avg_green = N.g + NE.g + E.g;
						avg_green/=3;
						avg_blue = N.b + NE.b +E.b;
						avg_blue /=3;


						avg_color = new Color(avg_red, avg_green, avg_blue);
						SetColorOf(x, z, avg_color);
						continue;
					} 

					if(z == size - 1) {
						//upper right corner, retrieve W, SW, and s
						W = ColorOf (WestOf (x,z));
						NW = ColorOf (SouthWestOf (x,z));
						N = ColorOf (SouthOf (x,z));

						avg_red = W.r + NW.r + N.r;
						avg_red /= 3;
						avg_green = W.g + NW.g + N.g;
						avg_green /= 3;
						avg_blue = W.b + NW.b + N.b;



						avg_color = new Color(avg_red, avg_green, avg_blue);
						SetColorOf(x, z, avg_color);
						continue;
					}


					//left border, retieve N, NE, E, SE, and S
					//Debug.Log ("("+x+","+z+")");
					 N = ColorOf (NorthOf (x,z));
					 NE = ColorOf (NorthEastOf (x,z));
					 E = ColorOf (EastOf(x,z));
					 SE = ColorOf (SouthEastOf (x,z));
					 S = ColorOf (SouthOf (x,z));

					 avg_red = N.r + NE.r + E.r + SE.r + S.r;
					avg_red /=5;
					 avg_green = N.g + NE.g + E.g + SE.g + S.g;
					avg_green /=5;
					 avg_blue = N.b + NE.b + E.g + SE.b + S.b;

					 avg_color = new Color(avg_red, avg_green, avg_blue);
					SetColorOf(x, z, avg_color);
					continue;
					                  
				}

				if(x == size-1) {

					if(z == 0) {
						//upper left, retrieve S, SE, E
						S = ColorOf (SouthOf (x, z));
						SE = ColorOf (SouthEastOf (x,z));
						E = ColorOf (EastOf (x,z));

						avg_red = S.r + SE.r + E.r;
						avg_red /= 3;
						avg_green = S.g + SE.g + E.g;
						avg_green /=3;
						avg_blue = S.b + SE.b + E.b;
						avg_blue /=3;


						avg_color = new Color(avg_red, avg_green, avg_blue);
						SetColorOf(x, z, avg_color);
						continue;
					}

					if(z == size-1) {
						//upper right, retrieve W, SW, and S
						W = ColorOf (WestOf (x,z));
						SW = ColorOf (SouthWestOf (x,z));
						S = ColorOf (SouthOf (x,z));

						avg_red = W.r + SW.r + S.r;
						avg_red/=3;
						avg_green =  W.g + SW.g + S.g;
						avg_green /=3;
						avg_blue = W.b + SW.b + S.b;
						avg_blue /=3;


						avg_color = new Color(avg_red, avg_green, avg_blue);
						SetColorOf(x, z, avg_color);
						continue;
					}


					//right border, retrieve N, NW, W, SW, and S
					 N = ColorOf (NorthOf (x,z));
					 NW = ColorOf (NorthWestOf (x,z));
					 W = ColorOf (WestOf (x,z));
					 SW = ColorOf (SouthWestOf (x,z));
					 S = ColorOf (SouthOf (x,z));

					 avg_red = N.r + NW.r + W.r + SW.r + S.r;
					avg_red/=5;
					 avg_green = N.g + NW.g + W.g + SW.g + S.g;
					avg_green /=5;
					 avg_blue = N.b + NW.b + W.b + SW.b + S.b;
					avg_blue /=5;

					 avg_color = new Color(avg_red, avg_green, avg_blue);
					SetColorOf(x, z, avg_color);
					continue;
				}

				if(z == 0) {
					//bottom border, retrieve W, NW, N, NE, and E
					 W = ColorOf (WestOf (x,z));
					 NW = ColorOf (NorthWestOf (x,z));
					 N = ColorOf (NorthOf (x,z));
					 NE = ColorOf (NorthEastOf (x,z));
					 E = ColorOf (EastOf (x,z));

					 avg_red = W.r + NW.r + N.r + NE.r + E.r;
					avg_red /=5;
					 avg_green = W.g + NW.g + N.g + NE.g + E.g;
					avg_green /=5;
					 avg_blue = W.b + NW.b + N.b + NE.b + E.b;


					 avg_color = new Color(avg_red, avg_green, avg_blue);
					SetColorOf(x, z, avg_color);
					continue;
				}

				if(z == size-1) {
					//top border, retrieve W, SW, S, SE, and E
					 W = ColorOf (WestOf (x,z));
					 SW = ColorOf (SouthWestOf (x,z));
					 S = ColorOf (SouthOf (x,z));
					 SE = ColorOf (SouthEastOf (x,z));
					 E = ColorOf (EastOf (x,z));

					 avg_red = W.r + SW.r + S.r + SE.r + E.r;
					avg_red /= 5;

					 avg_green = W.g + SW.g + S.g + SE.g + E.g;
					avg_green /= 5;
					 avg_blue = W.b + SW.b + S.b + SE.b + E.b;
					avg_blue /=5;

					 avg_color = new Color(avg_red, avg_green, avg_blue);
					SetColorOf(x, z, avg_color);
					continue;
					
				}
				 NW = ColorOf (NorthWestOf (x,z));
				 N = ColorOf (NorthOf (x,z));
				 NE = ColorOf (NorthEastOf (x,z));
				 E = ColorOf (EastOf (x,z));
				 S = ColorOf (SouthOf (x,z));
				 SE = ColorOf (SouthEastOf (x,z));
				 SW = ColorOf (SouthWestOf (x,z));
				 W = ColorOf (WestOf (x,z));

				 avg_red = NW.r + N.r + NE.r + E.r + SE.r + S.r + SW.r + W.r;
				avg_red /= 8;
				 avg_green = NW.g + N.g + NE.g + E.g + SE.g + S.g + SW.g + W.g;
				avg_green /=8;
				 avg_blue = NW.b + N.b + NE.b + E.b + SE.b + S.b + SW.b + W.b;
				avg_blue /= 8;



				 avg_color = new Color(avg_red, avg_green, avg_blue);
				SetColorOf(x, z, avg_color);

			}
		}

	}

	private void SetColorOf(int x, int y, Color c) {
		cubes [indexOf (x, y)].GetComponent<Renderer> ().material.color = c;
	}

	private Color ColorOf(int index) {
		//Debug.Log ("INDEX: " + index);
//		if (cubes [index] == null) {
//			Debug.Log ("WTF");
//		}
		return cubes[index].GetComponent<Renderer>().material.color;
	}

	private int NorthOf(int x, int y) {
		return indexOf (x, y + 1);
	}

	private int NorthEastOf(int x, int y) {
		return indexOf (x + 1, y + 1);
	}

	private int EastOf(int x, int y) {
		return indexOf(x+1, y);
	}

	private int SouthEastOf(int x, int y) {
		return indexOf (x+1, y-1);
	}

	private int SouthOf(int x, int y) {
		return indexOf(x, y-1);
	}

	private int SouthWestOf(int x, int y) {
		return indexOf(x-1, y-1);
	}

	private int WestOf(int x, int y) {
		return indexOf (x-1, y);
	}

	private int NorthWestOf(int x, int y) {
		return indexOf (x-1, y+1);
	}
	/*
	void Awake() {
		for (int x = 0; x < size; x++) {
			for(int z = 0; z < size; z++) {

				float height = Mathf.Sin (x * (TwoPI/size)) * Mathf.Sin(z *(TwoPI/size) )*magnitude;
//				height = Mathf.Sin (x+z*0.71f) * Mathf.Cos (x*0.76f+z*0.63f);
				GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
				Vector3 position = new Vector3(x,height,z);
				cube.transform.position = position;
				cube.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

				Renderer r = cube.GetComponent<Renderer>();

				//height will be between -1 and 1, a maximum range of 2f. Color intensity goes from 0 to 1, a maximum range of 1.
				//so we half the range of height to get a range from -0.5 and 0.5, and then add 0.5 to it to get our correct range of 0 - 1
				r.material.color = new Color(0, height/2.0f + 0.5f, 0);


				Vector3 fieldPos = new Vector3(x, 0, z)/fieldWidth;
				Vector2 delta = new Vector2(fieldPos.x, fieldPos.z) - circlePosition;
				float distanceFromCircle = Mathf.Abs(circleRadius - delta.magnitude); //this will go from -0.75 to 0 to 0.25


				float distanceToEdge = delta.magnitude - circleRadius;
				float edgeSharpness = 500;
				float intensity = Mathf.Clamp01 (1f-distanceToEdge*edgeSharpness);
				r.material.color = new Color(1*intensity, 1*intensity, 1*intensity);


////				//so at -0.001 or so
//				if(delta.magnitude <= circleRadius) {
//					//Debug.Log ("DING!");
////					Debug.Log (delta.magnitude);
//					float intensity = 1f;
//					r.material.color = new Color(1*intensity, 1*intensity, 1*intensity);
//				} else if(delta.magnitude < circleRadius*1.04f && delta.magnitude > circleRadius){
//
//					float intensity = (1 - (delta.magnitude - circleRadius))*0.6f;
//					r.material.color = new Color(1*intensity,1*intensity,1*intensity);
//				}
//				else if(delta.magnitude < circleRadius*1.06f && delta.magnitude > circleRadius*1.04f) {
//					float intensity = (1 - (delta.magnitude - circleRadius)) *0.4f;
//					r.material.color = new Color(1*intensity, 1*intensity, 1*intensity);
//				}
//				else {
//					r.material.color = Color.black;
//				}




				cube.AddComponent<MouseOverBehavior>();
//				cube.AddComponent<MoveUpAndDown>();
			}
		}
	}
	*/

}
