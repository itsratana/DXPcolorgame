using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverP2 : MonoBehaviour
{
    public static GameOverP2 instance {  get; private set; }

    public GameObject gameOverPanel;
    public bool isGameOver = false;
    public Button restartGameOver;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        Time.timeScale = 1.0f;
        gameOverPanel.SetActive(false);

        restartGameOver.onClick.AddListener(Restart);
    }
    public void GameOver()
    {
        if (!isGameOver)
        {
            gameOverPanel.SetActive(true);
            Time.timeScale = 0f;
            isGameOver = true;
        }
    }

    public void Restart()
    {
        gameOverPanel.SetActive(false);
        Time.timeScale = 1f;
        isGameOver = true;

        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);

        NetworkServer.DisconnectAll();
    }
}

