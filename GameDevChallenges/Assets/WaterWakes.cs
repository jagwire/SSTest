﻿using UnityEngine;
using System.Collections;

public class WaterWakes : MonoBehaviour {

	Mesh waterMesh;
	MeshFilter waterMeshFilter;
	float waterWidth = 3f;
	float gridSpacing = 0.1f;

	// Use this for initialization
	void Start () {
		waterMeshFilter = this.GetComponent<MeshFilter>();
		List<Vector3[]> height_tmp = GenerateWaterMesh.GenerateWater(waterMeshFilter, waterWidth, gridSpacing);	
		
		waterMesh = waterMeshFilter.mesh;
		
		BoxCollider boxCollider - this.GetComponent<BoxCollider>();
		boxCollider.center = new Vector3(waterWidth/2.0f, 0.0f, waterWidth/2.0f );
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
