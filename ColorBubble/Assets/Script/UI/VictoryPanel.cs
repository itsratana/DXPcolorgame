using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class VictoryPanel : MonoBehaviour
{
    public static VictoryPanel instance {  get; private set; }

    public GameObject victoryPanel;
    public bool hasWon = false;
    public Button restartGameOver;
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI scoreText;

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
        finalScoreText.text = ScoreManager.instance.finalScore.ToString();
        scoreText.text = ScoreManager.instance.score.ToString();
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
    }

    public void MoveToScene(int SceneID)
    {
        SceneManager.LoadScene(SceneID);
    }
}
