using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public static Timer instance {  get; private set; }

    [SerializeField] private GameOverP2 gameOver;

    public TextMeshProUGUI timer;
    public float gameTime;
    public float timeLeft;
    public float timeSpent;
    private void Awake()
    {
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
            GameOverP2.instance.GameOver();
            GameOverP2.instance.isGameOver = true;
        }
        
    }
}
