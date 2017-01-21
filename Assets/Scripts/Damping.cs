using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Damping {

    public static float Damp(float source, float target, float factor, float dt, float speed = 1)
    {
        float lerpFactor = dt * speed < float.Epsilon ? 0 : (dt * speed) / (dt * speed + factor);
        return Mathf.Lerp(source, target, lerpFactor);
    }

    public static Color Damp(Color source, Color target, float factor, float dt, float speed = 1)
    {
        float lerpFactor = dt * speed < float.Epsilon ? 0 : (dt * speed) / (dt * speed + factor);
        return Color.Lerp(source, target, lerpFactor);
    }

    public static Vector3 Damp(Vector3 source, Vector3 target, float factor, float dt, float speed = 1)
    {
        float lerpFactor = dt * speed < float.Epsilon ? 0 : (dt * speed) / (dt * speed + factor);
        return Vector3.Lerp(source, target, lerpFactor);
    }

    public static Quaternion Damp(Quaternion source, Quaternion target, float factor, float dt, float speed = 1)
    {
        float lerpFactor = dt * speed < float.Epsilon ? 0 : (dt * speed) / (dt * speed + factor);
        return Quaternion.Slerp(source, target, lerpFactor);
    }
}

