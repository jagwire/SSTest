using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GenerateWaterMesh : MonoBehaviour {
	public static List<Vector3[]> GenerateWater(MeshFilter waterMeshFilter, float size, float spacing) {
		int totalVertices = (int)Mathf.Round(size/spacing) + 1;
		
		List<Vector3[]> vertices2dArray = new List<Vector3[]>();
		
	}
}
