using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : MonoBehaviour
{


    public enum ColorChange
    {
        red, blue, green
    }
    public ColorChange newColor = ColorChange.red;

    private void Update()
    {
        //ChangeColor();
    }
    private void OnTriggerEnter(Collider collision)
    {
        PlayerII playerII = collision.gameObject.GetComponent<PlayerII>();
        if (playerII != null)
        {
            switch (newColor)
            {
                case ColorChange.red:
                    playerII.ChangeColor(playerII.colorPlayer[(int)Player.PlayerColor.red]);
                    //playerII.ChangeColor(0);
                    playerII.ChangePlayerColor(PlayerII.PlayerColor.red);
                    break;
                case ColorChange.blue:
                    playerII.ChangeColor(playerII.colorPlayer[(int)PlayerII.PlayerColor.blue]);
                    //playerII.ChangeColor(1);
                    playerII.ChangePlayerColor(PlayerII.PlayerColor.blue);
                    break;
                case ColorChange.green:
                    playerII.ChangeColor(playerII.colorPlayer[(int)PlayerII.PlayerColor.green]);
                    //playerII.ChangeColor(2);
                    playerII.ChangePlayerColor(PlayerII.PlayerColor.green);
                    break;
                default:
                    break;
            }
        }
    }
    /*public void ChangeColor()
    {
        if (colorChanger.Length > 0)
        {
            GetComponent<Renderer>().material.color = colorChanger[0];
        }
    }
    private void OnValidate()
    {
        ChangeColor();
    }*/
}
