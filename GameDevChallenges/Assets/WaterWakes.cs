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
	
	// Update is called once per frame
	void Update () {
	
	}
}
