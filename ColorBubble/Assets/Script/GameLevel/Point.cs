using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class Point : MonoBehaviour
{
    [SerializeField] PointType pointType;
    public enum PointType
    {
        red, blue, green
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3)
        {
            if (other.gameObject.TryGetComponent<Player>(out var player))
            {
                if (pointType == PointType.red)
                {
                    CheckPointType(player, Player.PlayerColor.red);
                }
                else if (pointType == PointType.blue) 
                {
                    CheckPointType(player, Player.PlayerColor.blue);
                }
                else
                {
                    CheckPointType(player, Player.PlayerColor.green);
                }
            }

            if (other.gameObject.TryGetComponent<PlayerII>(out var playerII))
            {
                if (pointType == PointType.red)
                {
                    CheckPointType(playerII, PlayerII.PlayerColor.red); 
                }
                else if (pointType == PointType.blue)
                {
                    CheckPointType(playerII, PlayerII.PlayerColor.blue);
                }
                else
                {
                    CheckPointType(playerII, PlayerII.PlayerColor.green);
                }
            }

            Destroy(gameObject);
        }
    }

    private void CheckPointType(Player player, Player.PlayerColor type)
    {
        if (player.currentColor == type)
        {
            //player.ScaleUp();
            ScoreManager.instance.AddPoint();
        }
        else
        {
            //player.ScaleDown();
            ScoreManager.instance.RemovePoint();
        }
    }

    private void CheckPointType(PlayerII playerII, PlayerII.PlayerColor type)
    {
        if (playerII.currentColor == type)
        {
            //player.ScaleUp();
            ScoreManager.instance.AddPoint();
        }
        else
        {
            //player.ScaleDown();
            ScoreManager.instance.RemovePoint();
        }
    }
}
