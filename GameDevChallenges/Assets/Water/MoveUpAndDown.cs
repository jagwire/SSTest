using UnityEngine;
using System.Collections;

public class MoveUpAndDown : MonoBehaviour {

	private bool started = false;
	// Update is called once per frame
	void Update () {
	
		if (started) {
			Vector3 currentPosition = transform.position;

			currentPosition.y = Time.deltaTime * Mathf.Sin (Time.realtimeSinceStartup)*10f;
			transform.position = currentPosition;
		}
	}
	private Vector3 savedPosition;
	public void go() {
		savedPosition = transform.position;
		started = true;
	}

	public void stop() {
		started = false;
		transform.position = savedPosition;

	}
}
