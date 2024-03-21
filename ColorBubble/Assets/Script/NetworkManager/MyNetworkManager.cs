using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class MyNetworkManager : NetworkManager
{
    public bool isGameStart = false;

    public List<PlayerII> players = new List<PlayerII>();

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);
        PlayerII player = conn.identity.GetComponent<PlayerII>();
        if (player != null)
        {
            player.SetUpID(players.Count);
            players.Add(player);
            if (players.Count == 1)
            {
                RefManager.Instance.cameraP1.playerTransform = players[0].transform;
            }
            else if (players.Count == 2)
            {
                RefManager.Instance.cameraP2.playerTransform = players[1].transform;
            }
        }

        if (numPlayers == 2)
        {
            isGameStart = true;
        }
    }
    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        PlayerII player = conn.identity.GetComponent<PlayerII>();
        players.Remove(player);
        base.OnServerDisconnect(conn);
    }

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

    public override void OnClientDisconnect()
    {
        if (RefManager.Instance.networkInputField)
        {
            RefManager.Instance.networkInputField.gameObject.SetActive(true);
        }
        if (RefManager.Instance.launchButton)
        {
            RefManager.Instance.launchButton.gameObject.SetActive(false);
        }
    }
}
