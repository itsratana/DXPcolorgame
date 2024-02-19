using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject menuPanel;
    public GameObject settingPanel; 
    public Button pauseButton;
    public Button resumeButton;
    public Button restartButton;
    public Button settingButton;
    private bool isMenuActive = true;

    void Start()
    {
        Time.timeScale = 1f;
        menuPanel.SetActive(false);
        settingPanel.SetActive(false);

        resumeButton.onClick.AddListener(Resume);
        restartButton.onClick.AddListener(Restart);
        pauseButton.onClick.AddListener(TogglePause);
        settingButton.onClick.AddListener(Setting);
    }
    void Update()
    {

    }

    public void OnClick()
    {
         menuPanel.SetActive(!menuPanel.activeSelf);
        //menuPanel.gameObject.SetActive(true);

        Time.timeScale = menuPanel.activeSelf ? 0f : 1f;
    }

    void TogglePause()
    {
        if (isMenuActive)
        {
            Pause();
        }
    }

    void Pause()
    {
        pauseButton.gameObject.SetActive(false);
        menuPanel.SetActive(true);
        Time.timeScale = 0f;
        isMenuActive = false;
    }

    void Resume()
    {
        pauseButton.gameObject.SetActive(true);
        menuPanel.SetActive(false);
        Time.timeScale = 1f;
        isMenuActive = true;
    }

    void Setting()
    {
        settingPanel.SetActive(!settingPanel.activeSelf);

        Time.timeScale = settingPanel.activeSelf ? 0f : 1f;
    }

    void Restart()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void MoveToScene(int SceneID)
    {
        SceneManager.LoadScene(SceneID);
    }
}
