using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leader : MonoBehaviour
{

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

    public float cameraRotationDamping = 0.3f;
    public float cameraPitch = 90f;

    public GameObject highlight;

    // Use this for initialization
    void Start()
    {
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
        HighlightClosestFriend();
        MaybeRecruitClosestFriend();
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
            if (Input.GetKey(KeyCode.Space) && targetWaypoint.nextAlt != null)
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
        var friends = GameObject.FindObjectsOfType<Friend>();
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

    void HighlightClosestFriend()
    {
        if (highlight != null)
        {
            if (closestFriend != null)
            {
                highlight.SetActive(true);
                highlight.transform.position = closestFriend.transform.position;
            }
            else
            {
                highlight.SetActive(false);
            }
        }
    }

    void MaybeRecruitClosestFriend()
    {
        if (closestFriend != null && Input.GetKeyUp(KeyCode.Space))
        {
            closestFriend.JoinBand();
        }
    }

    void TravelWithFriends()
    {
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
            friend.targetPosition = transform.position + followerRotation * new Vector3(x - y / 2.0f, 0, -y / triangleHeight) * friendDistance;
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
    }
}
