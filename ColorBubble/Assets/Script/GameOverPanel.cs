using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour
{
    [SerializeField] private Player player = null;
    public GameObject gameOverPanel;
    bool isGameOver = false;
    public Button restartGameOver;


    void Start()
    {
        Time.timeScale = 1.0f;
        gameOverPanel.SetActive(false);

        restartGameOver.onClick.AddListener(Restart);
    }

    void Update()
    {
        if (player.GetIsDead())
        {
            GameOver();
        }
    }
    void GameOver()
    {
        if (!isGameOver)
        {
            gameOverPanel.SetActive(true);
            Time.timeScale = 0f;
            isGameOver = true;
        }
    }

    void Restart()
    {
        gameOverPanel.SetActive(false);
        Time.timeScale = 1f;
        isGameOver = true;

        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void MoveToScene(int SceneID)
    {
        SceneManager.LoadScene(SceneID);
    }
}

