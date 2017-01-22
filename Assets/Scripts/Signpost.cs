using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Signpost : MonoBehaviour {
    public string flipAnimation = "signpost_z_rotate_rtl";
    public string unFlipAnimation = "signpost_z_rotate_ltr";

    public void Flip()
    {
        GetComponent<Animation>().PlayQueued(flipAnimation);
    }

    public void UnFlip()
    {
        GetComponent<Animation>().PlayQueued(unFlipAnimation);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
