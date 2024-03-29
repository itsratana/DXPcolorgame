using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3)
        {
            if (other.TryGetComponent(out PlayerII player))
            {
                if (!GameOverPanel.instance.isGameOverP1)
                {
                    if (player.playerID == 0)
                    {
                        VictoryPanel.instance.p1HasWon = true;
                    }
                }
                if (!GameOverPanel.instance.isGameOverP2)
                {
                    if (player.playerID == 1)
                    {
                        VictoryPanel.instance.p2HasWon = true;
                    }
                }
            }
            VictoryPanel.instance.Victory();
        }
    }
}
