using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3)
        {
            GameOverP2.instance.GameOver();
            GameOverP2.instance.isGameOver = true;

        }
    }
}
