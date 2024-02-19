using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    [SerializeField] private VictoryPanel victoryPanel;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3)
        {
            VictoryPanel.instance.Victory();
            VictoryPanel.instance.hasWon = true;
        }
    }
}
