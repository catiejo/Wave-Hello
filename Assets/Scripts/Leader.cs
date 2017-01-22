using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leader : MonoBehaviour
{
    public AudioClip[] helloClips;
    public float speed = 2f;
    public float friendDistance = 3f;
    public float maxRecruitingDistance = 10f;
    public Friend closestFriend;
    public List<Friend> recruitedFriends = new List<Friend>();
    public Waypoint targetWaypoint;
    public float waypointArrivalDistance = 2f;
    public Vector3 targetPosition;
    public float waypointDamping = 1f;
    public float rotationDamping = 0.5f;
    public Quaternion followerRotation;
    public Friend[] allFriends;

    public float friendXSeparationFactor = 0.75f;

    public Transform highlightArrow;
    public float friendArrowHeight = 0.4f;
    public float signArrowHeight = 0.75f;
    public Color friendArrowColor = Color.cyan;
    public Color signArrowColor = Color.yellow;

    public Vector3[] trailPoints;
    public int trailDefinition = 5;
    public int maxTrailLength = 100;
    public int nextTrailPoint = 0;

    public float cameraRotationDamping = 0.3f;
    public float cameraPitch = 90f;

    public GameObject highlight;

    // Use this for initialization
    void Start()
    {
        allFriends = GameObject.FindObjectsOfType<Friend>();
        trailPoints = new Vector3[maxTrailLength];

        FindClosestWaypoint();
        targetPosition = transform.position;
        followerRotation = transform.rotation;

        EventManager.OnJoinBand += OnJoinBand;
        EventManager.OnLeaveBand += OnLeaveBand;
    }

    void OnJoinBand(Friend friend)
    {
        recruitedFriends.Add(friend);
        friend.transform.parent = transform;
    }

    void OnLeaveBand(Friend friend)
    {
        recruitedFriends.Remove(friend);
        friend.transform.parent = null;
    }

    void FindClosestWaypoint()
    {
        targetWaypoint = null;
        var waypoints = FindObjectsOfType<Waypoint>();
        foreach (var waypoint in waypoints)
        {
            var distance = (waypoint.transform.position - transform.position).magnitude;
            if (targetWaypoint == null || distance < (targetWaypoint.transform.position - transform.position).magnitude)
            {
                targetWaypoint = waypoint;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        FollowWaypoints();
        TravelWithFriends();
        FindClosestFriend();
        UpdateHighlight();
        MaybeRecruitClosestFriend();
        MaybeToggleWaypoint();
    }

    void FollowWaypoints()
    {
		var waypointPosition = targetWaypoint != null ? targetWaypoint.transform.position : targetPosition;
		targetPosition = Damping.Damp (targetPosition, waypointPosition, waypointDamping, Time.deltaTime);
        var direction = (targetPosition - transform.position).normalized;
        Camera.main.transform.rotation = Damping.Damp(Camera.main.transform.rotation,
            Quaternion.LookRotation(direction, transform.up) * Quaternion.Euler(cameraPitch, 0, 0),
            cameraRotationDamping, Time.deltaTime);
        transform.Translate(direction * speed * Time.deltaTime);
        followerRotation = Damping.Damp(followerRotation, Quaternion.LookRotation(direction, transform.up), rotationDamping, Time.deltaTime);

		var offset = waypointPosition - transform.position;
        var distance = offset.magnitude;
        if (targetWaypoint != null && distance < waypointArrivalDistance)
        {
            if (targetWaypoint.nextAlt != null && targetWaypoint.flipped)
            {
                targetWaypoint = targetWaypoint.nextAlt;
            } else
            {
                targetWaypoint = targetWaypoint.next;
            }
        }
    }

    void FindClosestFriend()
    {
        closestFriend = null;
        var friends = allFriends;
        foreach (var friend in friends)
        {
            if (recruitedFriends.Contains(friend)) continue;
            if (!friend.canBeRecruited) continue;
            var distance = (friend.transform.position - transform.position).magnitude;
            if (distance > maxRecruitingDistance) continue;
            if (closestFriend == null || distance < (closestFriend.transform.position - transform.position).magnitude)
            {
                closestFriend = friend;
            }
        }
    }

    void UpdateHighlight()
    {
        if (highlight != null)
        {
            if (targetWaypoint != null && targetWaypoint.nextAlt)
            {
				UpdateCircle (false);
                highlight.SetActive(true);
                highlight.transform.position = targetWaypoint.sign != null ?
                    targetWaypoint.sign.transform.position : targetWaypoint.transform.position;
            }
            else
            {
                if (closestFriend != null)
                {
					UpdateCircle (true);
                    highlight.SetActive(true);
                    highlight.transform.position = closestFriend.transform.position;
                }
                else
                {
                    highlight.SetActive(false);
                }
            }
        }
    }

	void UpdateCircle(bool isFriend) {
        if (highlightArrow == null) return;
		var height = isFriend ? friendArrowHeight : signArrowHeight;
		var color = isFriend ? friendArrowColor: signArrowColor;
        highlightArrow.GetComponent<SpriteRenderer> ().color = color;
        highlightArrow.transform.localPosition = new Vector3 (0, height, 0);
	}

    void MaybeToggleWaypoint()
    {
        if (Input.GetKeyDown(KeyCode.Space) && targetWaypoint != null && targetWaypoint.nextAlt)
        {
            targetWaypoint.Toggle();
        }
    }

    void MaybeRecruitClosestFriend()
    {
        if (closestFriend != null && Input.GetKeyDown(KeyCode.Space))
        {
            var audioSource = GetComponent<AudioSource>();
            if (audioSource != null && helloClips.Length > 0)
            {
                audioSource.clip = helloClips[Random.Range(0, helloClips.Length)];
                audioSource.Play();
            }
            var animation = GetComponent<Animation>();
            if (animation != null)
            {
                animation.Play();
            }
            closestFriend.JoinBand();
        }
    }

    void TravelWithFriends()
    {
        var distance = (trailPoints[nextTrailPoint] - transform.position).magnitude;
        if (distance > friendDistance / trailDefinition)
        {
            nextTrailPoint = (nextTrailPoint + 1) % trailPoints.Length;
            trailPoints[nextTrailPoint] = transform.position;
        }
        int x = 0;
        int y = 0;
        foreach (var friend in recruitedFriends)
        {
            if (x == y)
            {
                y += 1;
                x = 0;
            }
            else
            {
                x += 1;
            }
            var triangleHeight = Mathf.Sqrt(2);
            var previousPoint = trailPoints[(trailPoints.Length + nextTrailPoint - (y - 1) * trailDefinition) % trailPoints.Length];
            var currentPoint = trailPoints[(trailPoints.Length + nextTrailPoint - y * trailDefinition) % trailPoints.Length];
            var direction = previousPoint - currentPoint;
            if (direction.magnitude < Mathf.Epsilon)
            {
                direction = followerRotation * Vector3.forward;
            }
            var offset = (x - y / 2.0f);
            friend.targetPosition = currentPoint + Quaternion.LookRotation(direction, Vector3.up) * new Vector3(Mathf.Pow(Mathf.Abs(offset), friendXSeparationFactor) * Mathf.Sign(offset), 0, 0) * friendDistance;
            Debug.DrawLine(currentPoint, friend.targetPosition, Color.red);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, targetPosition);
        if (targetWaypoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, targetWaypoint.transform.position);
        }

        foreach (var point in trailPoints)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(point, 0.1f);
        }
    }
}
