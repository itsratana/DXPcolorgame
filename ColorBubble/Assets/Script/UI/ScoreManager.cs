using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance { get; set; }

    public TextMeshProUGUI scoreText;

    [Header("Score")]
    public int score = 0;
    public int finalScore = 0;
    private int comboMultiplier = 1;
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
        finalScore = Mathf.FloorToInt(score * timeLeft);
    }
    public void UpdateScoreText()
    {
        scoreText.text = score.ToString();
    }

    public void AddPoint()
    {
        score += comboMultiplier;
        comboMultiplier++;
        UpdateScoreText();
    }

    public void RemovePoint()
    {
        score -= 1;
        comboMultiplier = 1;
        UpdateScoreText();
    }

    public void scoreComboMultiplier(int multiplier)
    {
        comboMultiplier *= multiplier;
        UpdateScoreText();
    }
}
