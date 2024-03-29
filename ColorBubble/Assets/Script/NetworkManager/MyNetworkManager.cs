using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

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
                        players[i].transform.position = RefManager.Instance.playerStartPositions[i].position;
                    }
                    else if (i == 1)
                    {
                        RefManager.Instance.cameraP2.playerTransform = players[i].transform;
                        players[i].transform.position = RefManager.Instance.playerStartPositions[i].position;
                    }
                    break;
                }
            }
        }
    }
    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {

        PlayerII player = conn.identity.GetComponent<PlayerII>();

        if (player != null)
        {
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i] == player)
                {
                    players[i] = null;
                }
            }
        }
        base.OnServerDisconnect(conn);
        /*
                if (player != null && players.Contains(player))
                {
                    players.Remove(player);
                }*/
    }

    public override void OnServerConnect(NetworkConnectionToClient conn)
    {
        if (isGameStart)
        {
            conn.Disconnect();
            return;
        }
        base.OnServerConnect(conn);
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
            if (player != null)
            {
                player.GameStart();
            }
        }
    }

    public void UpdateStartingCountdownText(float countdownTime)
    {
        RefManager.Instance.startingGameCountdown.text = Mathf.CeilToInt(countdownTime).ToString("0");
    }

    public override void OnStartClient()
    {
        if (RefManager.Instance.networkInputField)
        {
            RefManager.Instance.networkInputField.gameObject.SetActive(false);
        }
        if (RefManager.Instance.readyButton)
        {
            RefManager.Instance.readyButton.gameObject.SetActive(true);
        }
    }

    public override void OnClientDisconnect()
    {
        if (RefManager.Instance.identifierColor)
        {
            RefManager.Instance.identifierColor.color = new Color32(255, 255, 255, 0);
        }
        if (RefManager.Instance.networkInputField)
        {
            RefManager.Instance.networkInputField.gameObject.SetActive(true);
        }
        if (RefManager.Instance.readyButton)
        {
            RefManager.Instance.readyButton.gameObject.SetActive(false);
        }
        if (RefManager.Instance.launchButton)
        {
            RefManager.Instance.launchButton.gameObject.SetActive(false);
        }
    }
}
