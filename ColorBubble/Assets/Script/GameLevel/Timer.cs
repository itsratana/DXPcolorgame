using Mirror;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public static Timer instance { get; private set; }

    public MyNetworkManager myNetworkManager;

    [SerializeField] private GameOverPanel gameOver;

    public TextMeshProUGUI timer;
    public float gameTime;
    public float timeLeft;
    public float timeSpent;
    private void Awake()
    {
        myNetworkManager = (MyNetworkManager)NetworkManager.singleton;
        instance = this;
    }
    private void Start()
    {
        timeSpent = 0f;
    }
    void Update()
    {

    }
    public void TimerCountdown()
    {
        if (gameTime > 0f)
        {
            if (gameTime <= 0)
            {
                gameTime = 0f;
            }
            timeSpent += Time.deltaTime;
            gameTime -= Time.deltaTime;

            int minute = Mathf.FloorToInt(gameTime / 60);
            int second = Mathf.FloorToInt(gameTime % 60);
            int millisecond = Mathf.FloorToInt((gameTime * 100) % 100);
            timer.text = string.Format("{0:00}:{1:00}:{2:00}", minute, second, millisecond);
        }
        else
        {
            if (myNetworkManager.players[0] != null)
            {
                if (!VictoryPanel.instance.p1HasWon)
                {
                    GameOverPanel.instance.isGameOverP1 = true;
                }
            }
            if (myNetworkManager.players[1] != null)
            {
                if (!VictoryPanel.instance.p2HasWon)
                {
                    GameOverPanel.instance.isGameOverP2 = true;
                }
            }
            GameOverPanel.instance.GameOver();

            /*if (TryGetComponent(out PlayerII player))
            {
                if (player.playerID == 0)
                {
                    GameOverPanel.instance.isGameOverP1 = true;
                }
                if (player.playerID == 1)
                {
                    GameOverPanel.instance.isGameOverP2 = true;
                }
            }
            GameOverPanel.instance.GameOver();*/
        }

    }
}
