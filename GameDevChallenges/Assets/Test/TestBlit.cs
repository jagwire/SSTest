using UnityEngine;
using System.Collections;

public class TestBlit : MonoBehaviour {
	public Material kernel;
	public int size;
	private Texture2D waterHeightsTexture;
	private Texture2D terrainHeightsTexture;
	
	void Awake() {
		waterHeightsTexture = new Texture2D(size, size);
		terrainHeightsTexture = new Texture2D(size, size);
		
		for(int x = 0; x < size; x++) {
			for(int z = 0; z < size; z++) {
				waterHeightsTexture.SetPixel(x, z, Color.black);
				terrainHeightsTexture.SetPixel(x, z, Color.black);
			}
		}	
	}
	
	void Update () {
		Texture2D t = new Texture2D (size, size);
		RenderTexture rt = newTexture();

		kernel.SetTexture("waterHeights", waterHeightsTexture);
		kernel.SetTexture("terrainHeights", terrainHeightsTexture);
		kernel.SetFloat("x", 0.02f);
		Graphics.Blit (t, rt, kernel, -1);
	
	}

	RenderTexture newTexture() {
		return RenderTexture.GetTemporary(size, size,24, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Default);
	}
}
