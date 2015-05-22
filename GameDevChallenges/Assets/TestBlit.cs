using UnityEngine;
using System.Collections;

public class TestBlit : MonoBehaviour {
	public Material kernel;
	public int size;
	// Update is called once per frame
	void Update () {
		Texture2D t = new Texture2D (size, size);
		
		RenderTexture _rt = 
		
		
		
		RenderTexture rt = new RenderTexture (size, size, 16, RenderTextureFormat.ARGBFloat);

		Graphics.Blit (t, rt, kernel, -1);
	
	}
	
	RenderTexture newTexture() {
		return RenderTexture.GetTemporary(size, size,24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default);
	}
}
