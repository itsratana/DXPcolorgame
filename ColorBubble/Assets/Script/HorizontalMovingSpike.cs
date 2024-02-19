using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HorizontalMovingSpike : MonoBehaviour
{

    float speed = 5f;
    float minX = -4f; 
    float maxX = 2.5f; 

    private bool moveRight = true; 

    void Start()
    {
        MoveBackAndForth();
    }

    void MoveBackAndForth()
    {
        transform.DOMoveX(moveRight ? maxX : minX, speed)
            .SetEase(Ease.Linear)
            .SetSpeedBased() 
            .OnComplete(() =>
            {
                moveRight = !moveRight;

                float clampedX = Mathf.Clamp(transform.position.x, minX, maxX);
                transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);

                MoveBackAndForth();
            });
    }
}

