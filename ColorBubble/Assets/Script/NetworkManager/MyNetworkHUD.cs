using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
//using static BackendAPI;

public class MyNetworkHUD : MonoBehaviour
{
    [SerializeField] private MyNetworkManager manager;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Transform startGameButton;
    [SerializeField] private NetworkInfo networkInfo;

    //Test IP will remove later
    [SerializeField] private TextMeshProUGUI testIPText;
    [SerializeField] private ServerStarter serverStarter;
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private Transform loadingIndicator;
    [SerializeField] private TextMeshProUGUI tokenFailText;
    [SerializeField] private Transform loginButton;
    [SerializeField] private Transform startClientButton;
    //private string api = "https://192.168.88.45:3500/user/getUserInfo";

    private void Start()
    {
        //manager = RefManagerSingleton.Instance.myNetworkManager;
    }

    public void OnStopClient()
    {
        manager.StopClient();
    }

    public void OnServerOnly()
    {
        manager.StartServer();
        startGameButton.gameObject.SetActive(true);
    }

    public void OnStopServer()
    {
        //scoreManager.ResetEndingPopup();
        CallStopServer();
    }

    public void OnStopServerAfterEnding()
    {
        //scoreManager.ResetEndingPopup();
        Invoke(nameof(CallStopServer), 1f);
    }

    private void CallStopServer()
    {
        manager.StopServer();
        startGameButton.gameObject.SetActive(false);
        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(false);
        }
        serverStarter.StartServer();
    }

    public void UpdateInputField()
    {
        manager.networkAddress = inputField.text;
    }

    /*public void OnStartClientWhenNotConnectToServer()
    {
        StartCoroutine(GetValidateUserToken(networkInfo.ExtractToken(), Application.absoluteURL));
    }*/

    /*public IEnumerator GetValidateUserToken(string value, string url)
    {
        //manager.networkAddress = networkInfo.GetIPAddressFromUrl(url);
        //manager.StartClient();
        //yield return null;

        loadingIndicator.gameObject.SetActive(true);
        api = $"https://{networkInfo.GetIPAddressFromUrl(url)}:3500/user/getUserInfo";
        using (UnityWebRequest webRequest = UnityWebRequest.Get(api))
        {
            webRequest.downloadHandler = new DownloadHandlerBuffer();

            webRequest.certificateHandler = new BypassCertificate();

            webRequest.SetRequestHeader("user-token", value);

            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error: " + webRequest.error);
                loadingIndicator.gameObject.SetActive(false);
                tokenFailText.gameObject.SetActive(true);
                startClientButton.gameObject.SetActive(false);
                loginButton.gameObject.SetActive(true);
            }
            else
            {
                if (testIPText != null)
                {
                    testIPText.text = networkInfo.GetIPAddressFromUrl(url);
                }
                manager.networkAddress = networkInfo.GetIPAddressFromUrl(url);

                //manager.networkAddress = networkInfo.GetLocalIPAddressFromUrl();                
                manager.StartClient();
                tokenFailText.gameObject.SetActive(false);
                loadingIndicator.gameObject.SetActive(false);
            }
        }
    }*/

    public void NavigateToLoginPage()
    {
        string ipAddress = networkInfo.GetIPAddressFromUrl(Application.absoluteURL);
        Application.OpenURL($"https://{ipAddress}:3500/login?game_id=65bafc525047e3faffb9b3bd");
        //Application.OpenURL($"https://192.168.11.117:3500/login?game_id=65bafc525047e3faffb9b3bd");
    }

    public void TestOnStatClient()
    {
        //manager.networkAddress = networkInfo.GetLocalIPAddressFromUrl(Application.absoluteURL);
        //manager.networkAddress = "192.168.11.106";
        manager.networkAddress = "localhost";
        manager.StartClient();
    }
}