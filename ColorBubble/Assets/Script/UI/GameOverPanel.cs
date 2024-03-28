using TMPro;
using UnityEngine;

public class GameOverPanel : MonoBehaviour
{
    public static GameOverPanel instance { get; private set; }

    public bool isGameOverP1 = false;
    public bool isGameOverP2 = false;

    [Header("Victory Panel")]
    public GameObject gameOverPanelP1;
    public GameObject gameOverPanelP2;
    [Header("Player1 Score")]
    public TextMeshProUGUI finalScoreTextP1;
    public TextMeshProUGUI scoreTextP1;
    [Header("Player2 Score")]
    public TextMeshProUGUI finalScoreTextP2;
    public TextMeshProUGUI scoreTextP2;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        gameOverPanelP1.SetActive(false);
        gameOverPanelP2.SetActive(false);
    }
    private void Update()
    {
        finalScoreTextP1.text = ScoreManager.instance.finalScoreP1Lose.ToString();
        scoreTextP1.text = ScoreManager.instance.scoreP1.ToString();

        finalScoreTextP2.text = ScoreManager.instance.finalScoreP2Lose.ToString();
        scoreTextP2.text = ScoreManager.instance.scoreP2.ToString();
    }
    public void GameOver()
    {
        if (isGameOverP1)
        {
            gameOverPanelP1.SetActive(true);
        }
        if (isGameOverP2)
        {
            gameOverPanelP2.SetActive(true);
        }
    }
}

