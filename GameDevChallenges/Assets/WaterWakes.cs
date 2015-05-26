using UnityEngine;
using System.Collections;

public class WaterWakes : MonoBehaviour {

	Mesh waterMesh;
	MeshFilter waterMeshFilter;
	float waterWidth = 3f;
	float gridSpacing = 0.1f;

	// Use this for initialization
	void Start () {
		waterMeshFilter - this.GetComponent<MeshFilter>();
		List<Vector3[]> height_tmp = GenerateWaterMesh.GenerateWater(waterMeshFilter, waterWidth, gridSpacing);	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
