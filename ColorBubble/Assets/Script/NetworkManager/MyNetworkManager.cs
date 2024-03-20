using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class MyNetworkManager : NetworkManager
{
    public override void OnStartClient()
    {
        if (RefManager.Instance.networkInputField)
        {
            RefManager.Instance.networkInputField.gameObject.SetActive(false);
        }
        if (RefManager.Instance.launchButton)
        {
            RefManager.Instance.launchButton.gameObject.SetActive(true);
        }       
    }
}
