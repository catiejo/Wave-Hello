using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Friend : MonoBehaviour {

    public float minStay = 10f;
    public float maxStay = 20f;
    public float resetTime = 5f;
    public float positionDamping = 1.0f;
    public float maxSpeed = 2.0f;
    public Vector3 targetPosition;
    public Vector3 homePosition;
    public bool canBeRecruited = true;
    public Leader leader;

    public string friendType = "test";
    public AudioSource helloSound;
    public AudioSource joinSound;
    public AudioSource noSound;
    public AudioSource leaveSound;

    private bool _initialCanBeRecruited;

    public void Start()
    {
        joinSound = joinSound != null ? joinSound : GetComponent<AudioSource>();

        homePosition = transform.position;
        targetPosition = transform.position;
        _initialCanBeRecruited = canBeRecruited;
    }

    public void JoinBand()
    {
        FindObjectOfType<EventManager>().JoinBand(this);
        if (joinSound != null)
        {
            joinSound.Play();
        }
        var animation = GetComponent<Animation>();
        if (animation != null)
        {
            animation.Play();
        }
        Invoke("LeaveBand", Mathf.Lerp(minStay, maxStay, Random.value));
    }

    public void Update()
    {
        var nextPosition = Damping.Damp(transform.position, targetPosition, positionDamping, Time.deltaTime);
        var offset = nextPosition - transform.position;
        var distance = offset.magnitude;
        var direction = offset.normalized;
        transform.position = transform.position + direction * Mathf.Clamp(distance, 0, maxSpeed * Time.deltaTime);
    }

    public void Reset()
    {
        canBeRecruited = _initialCanBeRecruited;
    }

    public void LeaveBand()
    {
        FindObjectOfType<EventManager>().LeaveBand(this);
        targetPosition = homePosition;

        if (joinSound != null)
        {
            joinSound.Play();
        }
        var animation = GetComponent<Animation>();
        if (animation != null)
        {
            animation.Play();
        }

        Invoke("Reset", resetTime);
    }
}
