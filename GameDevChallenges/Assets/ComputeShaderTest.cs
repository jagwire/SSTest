using UnityEngine;
using System.Collections;
public class ComputeShaderTest : MonoBehaviour {

	
	void Awake() {
		Debug.LogError("SUPPORTS SHADERS? -> "+SystemInfo.supportsComputeShaders);	
	}
}
