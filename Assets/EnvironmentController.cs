using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentController : MonoBehaviour {
	public Leader leader;
	private string _currentBiome;

	void Update() {
		RaycastHit hit;
		if (Physics.Raycast (leader.transform.position, Vector3.forward, out hit)) {
			var tag = hit.collider.gameObject.tag;
			if (tag != _currentBiome) {
				_currentBiome = tag;
				Debug.Log ("changed to " + tag + " biome.");
			}
		}
	}
}
