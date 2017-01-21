using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Friend : MonoBehaviour {

    public string friendType = "test";
    public AudioSource helloSound;
    public AudioSource joinSound;
    public AudioSource noSound;
    public AudioSource leaveSound;

    public void Start()
    {
        joinSound = joinSound != null ? joinSound : GetComponent<AudioSource>();
    }

    public void JoinBand()
    {
        FindObjectOfType<EventManager>().JoinBand(friendType);
        if (joinSound != null)
        {
            joinSound.Play();
        }
        var animation = GetComponent<Animation>();
        if (animation != null)
        {
            animation.Play();
        }
    }

    public void LeaveBand()
    {
        FindObjectOfType<EventManager>().LeaveBand(friendType);
    }
}
