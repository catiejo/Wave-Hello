using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour {

    public Waypoint next;
    public Waypoint nextAlt;

    public bool flipped = false;
    public Signpost sign;

    // Use this for initialization
    void Start () {
        FindSign();
	}


    void FindSign()
    {
        if (sign == null && nextAlt != null)
        {
            Signpost bestSign = null;
            var minDistance = 0f;
            var signs = FindObjectsOfType<Signpost>();
            foreach (var sign in signs)
            {
                var distance = (sign.transform.position - transform.position).magnitude;
                if (bestSign == null || distance < minDistance)
                {
                    bestSign = sign;
                    minDistance = distance;
                }
            }
            sign = bestSign;
        }
    }

    public void Toggle()
    {
        if (flipped)
        {
            flipped = false;
            if (sign != null) sign.UnFlip();
        } else
        {
            flipped = true;
            if (sign != null) sign.Flip();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(transform.position, 0.35f);
        if (next != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, next.transform.position);
            var direction = (next.transform.position - transform.position).normalized;
            Gizmos.DrawCube(transform.position + direction * 1f, Vector3.one * 0.25f);
        }
        if (nextAlt != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, nextAlt.transform.position);
            var direction = (nextAlt.transform.position - transform.position).normalized;
            Gizmos.DrawCube(transform.position + direction * 1f, Vector3.one * 0.25f);
        }
    }
}
