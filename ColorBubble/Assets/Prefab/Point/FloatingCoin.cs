using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingCoin : MonoBehaviour
{
    public float rotateSpeed = 1f;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0f, rotateSpeed * Time.deltaTime / 0.01f, 0f, Space.Self);
    }
}
