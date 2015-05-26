﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GenerateWaterMesh : MonoBehaviour {
	public static List<Vector3[]> GenerateWater(MeshFilter waterMeshFilter, float size, float spacing) {
		int totalVertices = (int)Mathf.Round(size/spacing) + 1;
		
		List<Vector3[]> vertices2dArray = new List<Vector3[]>();
		List<int> tris = new List<int>();
		
		for(int z = 0; z < totalVertices; z++) {
			vertices2dArray.Add(new Vector3[totalVertices]);
			for(int x = 0; x < totalVertices; x++) {
				Vector3 current_point = new Vector3();
				current_point.x = x*spacing;
				current_point.y = 0f;
				current_point.z = z*spacing;
				
				vertices2dArray[z][x] = current_point;
				
				if(x <= 0 || z <= 0) { continue; }
				
				tris.Add(x + z * totalVertices);
				tris.Add(x+(z-1)*totalVertices);
				tris.Add((x-1) + (z-1) *totalVertices);
				
				tris.Add(x+z*totalVertices);
				tris.Add((x-1) + (z-1)*totalVertices);
				tris.Add((x-1) + z*totalVertices);	
			}
		}
		
		Vector3[] unfolded_verts = new Vector3[totalVertices*totalVertices];
		for(int i = 0; i < vertices2dArray.Count; i++) {
			vertices2dArray[i].CopyTo(unfolded_verts, i*totalVertices);
		}
		
		
		Mesh waterMesh = new Mesh();
		waterMesh.vertices = unfolded_verts;
		waterMesh.triangles = tris.ToArray();
		waterMesh.RecalculateBounds();
		waterMesh.RecalculateNormals();
		waterMesh.name = "WaterMesh";
		
		
		
		
		
	}
}
