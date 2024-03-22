using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance { get; set; }

    public TextMeshProUGUI scoreText;

    [Header("P1 Score")]
    public int scoreP1 = 0;
    public int finalScoreP1 = 0;
    private int comboMultiplierP1 = 1;

    [Header("P2 Score")]
    public int scoreP2 = 0;
    public int finalScoreP2 = 0;
    private int comboMultiplierP2 = 1;

    private float timeLeft;
    
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {

    }
    private void Update()
    {
        UpdateScoreText();
        CalculateFinalScore();
    }
    public void CalculateFinalScore()
    {
        timeLeft = Timer.instance.gameTime;
        finalScoreP1 = Mathf.FloorToInt(scoreP1 * timeLeft);
        finalScoreP2 = Mathf.FloorToInt(scoreP2 * timeLeft);
    }
    public void UpdateScoreText()
    {
        scoreText.text = scoreP1.ToString();
        scoreText.text = scoreP2.ToString();
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
        scoreP1 -= 1;
        comboMultiplierP1 = 1;
        scoreP2 -= 1;  
        comboMultiplierP2 = 1;
        UpdateScoreText();
    }

    public void scoreComboMultiplier(int multiplier)
    {
        comboMultiplierP1 *= multiplier;
        comboMultiplierP2 *= multiplier;
        UpdateScoreText();
    }
}
