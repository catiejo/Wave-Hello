using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UniformScaleAnimation : MonoBehaviour {

	public AnimationCurve scaleCurve;
    public static float globalBPM = 30;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.localScale = scaleCurve.Evaluate((Time.time % (60.0f / globalBPM)) * globalBPM / 60.0f) * Vector3.one;
	}
}
