using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RestartButton : MonoBehaviour
{
    public MyNetworkHUD myNetworkHUD;
    public MyNetworkManager myNetworkManager;
    public GameObject restartWarning;
    public Button serverRestartButton;
    public Button restartServer;
    public Button cancelServerRestart;

    private void Awake()
    {
        myNetworkManager = (MyNetworkManager)NetworkManager.singleton;
    }

    private void Start()
    {
        restartServer.onClick.AddListener(Restart);
        serverRestartButton.onClick.AddListener(OnClick);
        cancelServerRestart.onClick.AddListener(CancelAction);
    }
    public void OnClick()
    {
        restartWarning.SetActive(true);
    }
    public void CancelAction()
    {
        restartWarning.SetActive(false);
    }
    public void Restart()
    {       
        NetworkServer.DisconnectAll();
        myNetworkManager.isGameStart = false;

        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}
