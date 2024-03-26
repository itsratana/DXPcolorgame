
using UnityEngine;

public class RotateObjects : MonoBehaviour
{
    public float rotationSpeed = 30.0f;
    public bool isForward = true;
    void Update()
    {
        if (isForward)
        {
            transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime / 0.01f, Space.Self);
        }
        else
        {
            transform.Rotate(-Vector3.forward * rotationSpeed * Time.deltaTime / 0.01f, Space.Self);
        }

    }
}
