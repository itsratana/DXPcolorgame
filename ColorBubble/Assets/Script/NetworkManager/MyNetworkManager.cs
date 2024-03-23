using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class MyNetworkManager : NetworkManager
{
    public bool isGameStart = false;

    public List<PlayerII> players = new List<PlayerII>();

    private Coroutine startingGameCoroutine;

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);
        PlayerII player = conn.identity.GetComponent<PlayerII>();
        if (player != null)
        {
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i] == null)
                {
                    players[i] = player;
                    player.SetUpID(i);
                    if (i == 0)
                    {
                        RefManager.Instance.cameraP1.playerTransform = players[i].transform;
                    }
                    else if (i == 1)
                    {
                        RefManager.Instance.cameraP2.playerTransform = players[i].transform;
                    }
                    break;
                }
            }
        }
    }
    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        base.OnServerDisconnect(conn);
        PlayerII player = conn.identity.GetComponent<PlayerII>();

        if (player != null && players.Contains(player))
        {
            players.Remove(player);
        }
    }

    public void PlayerReadyCheck()
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i] != null)
            {
                if (!players[i].isReady)
                {
                    RefManager.Instance.startingGameCountdown.gameObject.SetActive(false);                    
                    if (startingGameCoroutine != null)
                    {
                        StopCoroutine(startingGameCoroutine);
                    }
                    return;
                }
            }
        }
        RefManager.Instance.startingGameCountdown.gameObject.SetActive(true);
        startingGameCoroutine = StartCoroutine(StartingGameCountdown());
    }

    IEnumerator StartingGameCountdown()
    {
        int countdown = 3;
        while (countdown >= 0)
        {
            UpdateStartingCountdownText(countdown);
            yield return new WaitForSeconds(1);
            countdown--;
        }
        RefManager.Instance.startingGameCountdown.gameObject.SetActive(false);
        isGameStart = true;

        foreach (PlayerII player in players)
        {
            player.GameStart();
        }
    }

    public void UpdateStartingCountdownText(float countdownTime)
    {
        RefManager.Instance.startingGameCountdown.text = Mathf.CeilToInt(countdownTime).ToString("0");
    }

    public override void OnStartClient()
    {
        Debug.Log("Start Client");
        if (RefManager.Instance.networkInputField)
        {
            RefManager.Instance.networkInputField.gameObject.SetActive(false);
        }
        if (RefManager.Instance.readyButton)
        {
            Debug.Log("Set ready button to true");
            RefManager.Instance.readyButton.gameObject.SetActive(true);
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
