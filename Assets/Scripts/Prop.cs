using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Prop : MonoBehaviour {

	// Use this for initialization
	void Start () {
        this.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up * 200, Vector3.down, out hit, 400))
        {
            transform.position = hit.point;
        }
	}
}
