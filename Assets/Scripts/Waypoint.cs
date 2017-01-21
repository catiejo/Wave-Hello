using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour {

    public Waypoint next;
    public Waypoint nextAlt;

    // Use this for initialization
    void Start () {
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
