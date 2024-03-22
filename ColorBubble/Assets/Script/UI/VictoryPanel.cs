using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Mirror;

public class VictoryPanel : MonoBehaviour
{
    public static VictoryPanel instance {  get; private set; }

    public GameObject victoryPanel;
    public bool hasWon = false;
    public Button restartGameOver;
    public TextMeshProUGUI finalScoreTextP1;
    public TextMeshProUGUI scoreTextP1;
    public TextMeshProUGUI finalScoreTextP2;
    public TextMeshProUGUI scoreTextP2;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        Time.timeScale = 1.0f;
        victoryPanel.SetActive(false);

        restartGameOver.onClick.AddListener(Restart);
    }
    private void Update()
    {
        finalScoreTextP1.text = ScoreManager.instance.finalScoreP1.ToString();
        scoreTextP1.text = ScoreManager.instance.scoreP1.ToString();

        finalScoreTextP2.text = ScoreManager.instance.finalScoreP2.ToString();
        scoreTextP2.text = ScoreManager.instance.scoreP2.ToString();
    }

    public void Victory()
    {
        if (!hasWon)
        {
            victoryPanel.SetActive(true);
            Time.timeScale = 0f;
            hasWon = true;
        }
    }

    public void Restart()
    {
        victoryPanel.SetActive(false);
        Time.timeScale = 1f;
        hasWon = true;

        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);

        NetworkServer.DisconnectAll();
    }
}
