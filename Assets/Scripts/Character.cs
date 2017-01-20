using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

	public AnimationCurve scaleCurve;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.localScale = scaleCurve.Evaluate(Time.time % 1) * Vector3.one;
	}
}
