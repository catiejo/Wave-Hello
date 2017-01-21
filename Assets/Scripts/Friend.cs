using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Friend : MonoBehaviour {

    public string friendType = "test";

    public void JoinBand()
    {
        FindObjectOfType<EventManager>().JoinBand(friendType);
    }

    public void LeaveBand()
    {
        FindObjectOfType<EventManager>().LeaveBand(friendType);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
