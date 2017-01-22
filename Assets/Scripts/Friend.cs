using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EFriendType {
    UNKNOWN,
    FOREST,
    DESERT,
}

[System.Serializable]
public class FriendRequirements
{
    public EFriendType type;
    public int count;
    public int level;
}

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
    public float joinDelay = 1.0f;
    public float joinChance = 0.7f;

    public EFriendType friendType = EFriendType.UNKNOWN;
    public AudioSource helloSound;
    public AudioSource joinSound;
    public AudioSource noSound;
    public AudioSource leaveSound;

    public AnimationClip joinAnimation;
    public AnimationClip denyAnimation;

    public int level;
    public FriendRequirements[] requirements;

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
        canBeRecruited = false;
        StartCoroutine(Joining());
    }

    public bool RequirementsFulfilled()
    {
        foreach (var requirement in requirements)
        {
            int count = 0;
            foreach (var friend in leader.recruitedFriends)
            {
                if (friend.friendType == requirement.type && friend.level == requirement.level)
                {
                    count += 1;
                }
            }
            if (count < requirement.count)
            {
                return false;
            }
        }
        return true;
    }

    public IEnumerator Joining()
    {
        yield return new WaitForSeconds(joinDelay);

        if (Random.value < joinChance || !RequirementsFulfilled())
        {
            FindObjectOfType<EventManager>().JoinBand(this);
            if (joinSound != null)
            {
                joinSound.Play();
            }
            var animation = GetComponent<Animation>();
            if (animation != null && joinAnimation != null)
            {
                animation.clip = joinAnimation;
                animation.Play();
            }
            Invoke("LeaveBand", Mathf.Lerp(minStay, maxStay, Random.value));
        } else
        {
            var animation = GetComponent<Animation>();
            if (animation != null && denyAnimation != null)
            {
                animation.clip = denyAnimation;
                animation.Play();
            }
            if (noSound != null)
            {
                noSound.Play();
            }
            Invoke("Reset", resetTime);
        }
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

        if (leaveSound != null)
        {
            leaveSound.Play();
        }
        Invoke("Reset", resetTime);
    }
}
