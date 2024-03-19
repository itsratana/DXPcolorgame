using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    public float rotationSpeed = 100f;
    [SerializeField] private float minRotation = -30f;
    [SerializeField] private float maxRotation = 30f;

    [SerializeField] LineRenderer lineRenderer;


    private float accumulatedRotation = 0f;
    public float GetAccumulatedRotation()
    {
        return accumulatedRotation;
    }
    private void Start()
    {
        
    }
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector3 acceleration = Input.acceleration;

        float rotationAmount = horizontalInput * rotationSpeed * Time.deltaTime;
        accumulatedRotation += rotationAmount;

        float accelerationRotation = acceleration.x * rotationSpeed * Time.deltaTime;
        accumulatedRotation += accelerationRotation;

        accumulatedRotation = Mathf.Clamp(accumulatedRotation, minRotation, maxRotation);

        transform.rotation = Quaternion.Euler(0f, accumulatedRotation, 0f);
    }
}
