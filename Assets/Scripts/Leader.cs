using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leader : MonoBehaviour {

    public float speed = 2f;
    public float friendDamping = 0.5f;
    public float friendDistance = 3f;
    public float maxRecruitingDistance = 10f;
    public Friend closestFriend;
    public List<Friend> recruitedFriends = new List<Friend>();

    public GameObject highlight;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        TravelWithFriends();
        FindClosesFriend();
        HighlightClosestFriend();
        MaybeRecruitClosestFriend();
	}

    void FindClosesFriend()
    {
        closestFriend = null;
        var friends = GameObject.FindObjectsOfType<Friend>();
        foreach (var friend in friends)
        {
            if (recruitedFriends.Contains(friend)) continue;
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
            } else
            {
                highlight.SetActive(false);
            }
        }
    }

    void MaybeRecruitClosestFriend()
    {
        if (closestFriend != null && Input.GetKeyUp(KeyCode.Space))
        {
            recruitedFriends.Add(closestFriend);
            closestFriend.transform.parent = transform;
        }
    }

    void TravelWithFriends()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
        int x = 0;
        int y = 0;
        foreach (var friend in recruitedFriends)
        {
            if (x == y)
            {
                y += 1;
                x = 0;
            } else
            {
                x += 1;
            }
            var triangleHeight = Mathf.Sqrt(2);
            var targetPosition = transform.position + transform.rotation * new Vector3(x - y / 2.0f, -y / triangleHeight, 0) * friendDistance;
            friend.transform.position = Damping.Damp(friend.transform.position, targetPosition, friendDamping, Time.deltaTime);
        }
    }
}
