using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;

public class ScoreManager : MonoBehaviour
{
    private MyNetworkManager m_NetworkManager;
    public static ScoreManager instance { get; set; }

    public TextMeshProUGUI scoreTextP1;
    public TextMeshProUGUI scoreTextP2;

    [Header("P1 Score")]
    public int scoreP1 = 0;
    public int finalScoreP1Win = 0;
    public int finalScoreP1Lose = 0;
    public float p1Distance;
    private int comboMultiplierP1 = 1;

    [Header("P2 Score")]
    public int scoreP2 = 0;
    public int finalScoreP2Win = 0;
    public int finalScoreP2Lose = 0;
    public float p2Distance;
    private int comboMultiplierP2 = 1;

    public float timeLeft;

    private void Awake()
    {
        instance = this;
        m_NetworkManager = (MyNetworkManager)NetworkManager.singleton;
    }
    private void Start()
    {

    }
    private void Update()
    {
        UpdateScoreText();
        CalculateFinalScore();

        if (m_NetworkManager.players[0] != null)
        {
            p1Distance = m_NetworkManager.players[0].CalculateDistance();
        }
        if (m_NetworkManager.players[1] != null)
        {
            p2Distance = m_NetworkManager.players[1].CalculateDistance();
        }
    }
    public void CalculateFinalScore()
    {
        timeLeft = Timer.instance.gameTime;

        if (!VictoryPanel.instance.p1HasWon)
        {
            finalScoreP1Win = Mathf.FloorToInt(scoreP1 + (timeLeft * 0.5f) + (p1Distance * 0.1f));
        }
        if (!VictoryPanel.instance.p2HasWon)
        {
            finalScoreP2Win = Mathf.FloorToInt(scoreP2 + (timeLeft * 0.5f) + (p2Distance * 0.1f));
        }

        if (!GameOverPanel.instance.isGameOverP1)
        {
            finalScoreP1Lose = Mathf.FloorToInt(scoreP1 + (p1Distance * 0.1f));
        }
        if (!GameOverPanel.instance.isGameOverP2)
        {
            finalScoreP2Lose = Mathf.FloorToInt(scoreP2 + (p1Distance * 0.1f));
        }
    }
    public void UpdateScoreText()
    {
        scoreTextP1.text = scoreP1.ToString();
        scoreTextP2.text = scoreP2.ToString();
    }

    public void AddPoint(int playerID)
    {
        switch (playerID)
        {
            case 0:
                scoreP1 += comboMultiplierP1;
                comboMultiplierP1++;
                break;
            case 1:
                scoreP2 += comboMultiplierP2;
                comboMultiplierP2++;
                break;
        }

        UpdateScoreText();
    }

    public void RemovePoint(int playerID)
    {
        switch (playerID)
        {
            case 0:
                scoreP1 = Mathf.Max(0, scoreP1 - 1);
                comboMultiplierP1 = 1;
                break;
            case 1:
                scoreP2 = Mathf.Max(0, scoreP2 - 1);
                comboMultiplierP2 = 1;
                break;
        }
        UpdateScoreText();
    }

    public void scoreComboMultiplier(int multiplier)
    {
        comboMultiplierP1 *= multiplier;
        comboMultiplierP2 *= multiplier;
        UpdateScoreText();
    }
}
