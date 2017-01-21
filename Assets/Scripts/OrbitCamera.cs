using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class OrbitCamera : MonoBehaviour {

    public float distance = 5f;
	
	// Update is called once per frame
	void Update () {
        transform.localPosition = transform.rotation * Vector3.back * distance;		
	}
}
