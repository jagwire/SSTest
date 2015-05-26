using UnityEngine;
using System.Collections;

public class MouseOverBehavior : MonoBehaviour {
	
	void OnMouseOver() {
//		this.gameObject.AddComponent<MoveUpAndDown> ();
		transform.localScale = new Vector3 (1.5f, 1.5f, 1.5f);
//		GetComponent<MoveUpAndDown> ().go ();
	}

	void OnMouseExit() {
		transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
//		GetComponent<MoveUpAndDown> ().stop ();
//		Destroy (this.gameObject.GetComponent<MoveUpAndDown> ());
	}
}
