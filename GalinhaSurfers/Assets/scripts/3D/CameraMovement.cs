using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 4.52f, -5.0f);
    public float smoothSpeed = 0.125f; // suavidade do movimento
    public Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = target.position + offset;
            transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);
        }
    }
}
