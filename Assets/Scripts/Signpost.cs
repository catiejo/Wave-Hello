using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Signpost : MonoBehaviour {
    public string flipAnimation = "signpost1_z_rotate_ltr";
    public string unFlipAnimation = "signpost1_z_rotate_rtl";
    public bool inverse = false;

    public void Start ()
    {
        UnFlip();
    }

    public void Flip()
    {
        GetComponent<Animation>().PlayQueued(inverse ? flipAnimation : unFlipAnimation);
    }

    public void UnFlip()
    {
        GetComponent<Animation>().PlayQueued(inverse ? unFlipAnimation : flipAnimation);
    }
}
