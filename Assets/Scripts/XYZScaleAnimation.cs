using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XYZScaleAnimation : MonoBehaviour {

	public AnimationCurve xScaleCurve;
    public AnimationCurve yScaleCurve;
    public AnimationCurve zScaleCurve;
    public static float globalBPM = 30;

	// Update is called once per frame
	void Update () {
        var bpmFactor = 60.0f / globalBPM;
        var xScale = xScaleCurve.Evaluate((Time.time % bpmFactor) / bpmFactor);
        var yScale = yScaleCurve.Evaluate((Time.time % bpmFactor) / bpmFactor);
        var zScale = zScaleCurve.Evaluate((Time.time % bpmFactor) / bpmFactor);
        this.transform.localScale = new Vector3(xScale, yScale, zScale);
	}
}
