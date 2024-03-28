using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Mirror;
using System;

public class VictoryPanel : MonoBehaviour
{
    public static VictoryPanel instance {  get; private set; }

    public bool p1HasWon = false;
    public bool p2HasWon = false;

    [Header("Victory Panel")]
    public GameObject victoryPanelP1;
    public GameObject victoryPanelP2;
    [Header("Player1 Score")]
    public TextMeshProUGUI finalScoreTextP1;
    public TextMeshProUGUI scoreTextP1;
    [Header("Player2 Score")]
    public TextMeshProUGUI finalScoreTextP2;
    public TextMeshProUGUI scoreTextP2;
    [Header("Timer")]
    public TextMeshProUGUI timerTextP1;
    public TextMeshProUGUI timerTextP2;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        victoryPanelP1.SetActive(false);
        victoryPanelP2.SetActive(false);
    }
    private void Update()
    {
        int minute = Mathf.FloorToInt(Timer.instance.timeSpent / 60);
        int second = Mathf.FloorToInt(Timer.instance.timeSpent % 60);

        finalScoreTextP1.text = ScoreManager.instance.finalScoreP1Win.ToString();
        scoreTextP1.text = ScoreManager.instance.scoreP1.ToString();
        if (!p1HasWon)
        {
            timerTextP1.text = string.Format("{0:00}:{1:00}", minute, second);
        }
        
        finalScoreTextP2.text = ScoreManager.instance.finalScoreP2Win.ToString();
        scoreTextP2.text = ScoreManager.instance.scoreP2.ToString();
        if(!p2HasWon)
        {
            timerTextP2.text = string.Format("{0:00}:{1:00}", minute, second);

        }
    }

    public void Victory()
    {
        if (p1HasWon)
        {
            victoryPanelP1.SetActive(true);
        }
        if (p2HasWon)
        {
            victoryPanelP2.SetActive(true);
        }
    }

    public void Restart()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);

        NetworkServer.DisconnectAll();
    }
}
