using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
// Flat shadow that orient around Y according to the camera rotation.
public class BillboardShadows : MonoBehaviour {
    void Update()
    {
        var camera = Camera.main;
        if (camera == null) return;
        var forward = camera.transform.rotation * Vector3.forward;
        forward.y = 0;
        transform.LookAt(transform.position + Vector3.down, forward);
    }
}
