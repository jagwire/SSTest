using UnityEngine;
using System.Collections;

public class TerrainMouseOver : MonoBehaviour {

	public GameObject marker;

	void OnMouseOver() {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		RaycastHit hitInfo;
		if(Physics.Raycast(ray, out hitInfo)) {
			Vector3 position = hitInfo.point;
			marker.transform.position = position;

		}
	}
}
