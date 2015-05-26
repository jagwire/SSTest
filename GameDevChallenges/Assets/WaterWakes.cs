using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class WaterWakes : MonoBehaviour {

	Mesh waterMesh;
	MeshFilter waterMeshFilter;
	float waterWidth = 3f;
	float gridSpacing = 0.1f;

	//part 2
	public float alpha = 0.9f;
	int P = 8; //kernel size
	float g = -9.81f; //gravity
	float [,] storedKernelArray;




	void Start () {
		waterMeshFilter = this.GetComponent<MeshFilter>();
		List<Vector3[]> height_tmp = GenerateWaterMesh.GenerateWater(waterMeshFilter, waterWidth, gridSpacing);	
		
		waterMesh = waterMeshFilter.mesh;
		
		BoxCollider boxCollider = this.GetComponent<BoxCollider>();
		boxCollider.center = new Vector3(waterWidth/2.0f, 0.0f, waterWidth/2.0f );
		boxCollider.size = new Vector3(waterWidth, 0.1f, waterWidth);
		
		transform.position = new Vector3(-waterWidth/2.0f, 0f, -waterWidth/2.0f);
		
		//part 2
		storedKernelArray = new float[P*2+1, P*2+1];
		PrecomputeKernelValues();
	}
	
	void PrecomputeKernelValues() {
		
		float G_zero = CalculateG_zero();
		
		for(int k = -P; k <= P; k++) {
			for(int l = -P; l <= P; l++) {
				storedKernelArray[k+P, l+P] = CalculateG((float)k, (float)l, G_zero);
			}
		}
	}
	
	private float CalculateG(float k, float l, float G_zero) {
		float delta_q = 0.001f;
		float sigma = 1f;
		float r = Mathf.Sqrt(k*k+l*l);
		
		float G = 0f;
		for(int n = 1; n <= 10000; n++) {
			float q_n = ((float) n*delta_q);
			float q_n_square = q_n*q_n;
			
			G+= q_n_square * Mathf.Exp(-sigma*q_n_square) * BesselFunction(q_n * r);
			
		}
		
		G /= G_zero;
		return G;
		
	}
	
	private float CalculateG_zero() {
		float delta_q = 0.001f;
		float sigma = 1f;
		float G_zero = 0f;
		
		for(int n = 1; n <= 10000; n++) {
			float q_n_square = ((float)n*delta_q) * ((float)n *delta_q);
			G_zero += q_n_square * Mathf.Exp(-sigma*q_n_square);
		}
		
		return G_zero;
	}
	
	
	
	// Update is called once per frame
	void Update () {
	
	}
}
