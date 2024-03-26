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

    public GameObject victoryPanelP1;
    public GameObject victoryPanelP2;
    public bool hasWon = false;
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
        //Time.timeScale = 1.0f;
        victoryPanelP1.SetActive(false);
        victoryPanelP2.SetActive(false);
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
            victoryPanelP1.SetActive(true);
            victoryPanelP2.SetActive(true);
            //Time.timeScale = 0f;
            hasWon = true;
        }
    }

    public void Restart()
    {
        victoryPanelP1.SetActive(false);
        victoryPanelP2.SetActive(false);
        //Time.timeScale = 1f;
        hasWon = true;

        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);

        NetworkServer.DisconnectAll();
    }
}
