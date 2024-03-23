using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform playerTransform;
    public float smoothTime = 0.3f; 
    public Vector3 offset;

    private Vector3 velocity = Vector3.zero;
    void FixedUpdate()
    {
        if (playerTransform != null)
        {
            Vector3 targetPosition = playerTransform.position + offset;

            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
    }
}
