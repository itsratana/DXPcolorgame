using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button quitButton;
    public void MoveToScene(int SceneID)
    {
        SceneManager.LoadScene(SceneID);
    }

    private void Start()
    {
        quitButton.onClick.AddListener(Quit);
    }
    public void Quit()
    {
        Application.Quit();
    }
}
