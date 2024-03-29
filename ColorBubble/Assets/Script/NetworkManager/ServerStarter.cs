using Mirror;
using UnityEngine;

public class ServerStarter : MonoBehaviour
{
    MyNetworkManager myNetworkManager;
    [SerializeField] private MyNetworkHUD _myNetworkHUD = null;
    //[SerializeField] private Transform _titleGame = null;
    [SerializeField] private Transform _qrCode = null;
    //[SerializeField] private NetworkInfo networkInfo = null;

    private void Start()
    {
        myNetworkManager = (MyNetworkManager)NetworkManager.singleton;
        StartServer();
    }
    private void Update()
    {
        if (myNetworkManager.isGameStart)
        {
            _qrCode.gameObject.SetActive(false);
        }
    }

    public void StartServer()
    {
        //_titleGame.gameObject.SetActive(true);
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            if (_qrCode != null)
            {
                _qrCode.gameObject.SetActive(true);
            }
            _myNetworkHUD.OnServerOnly();
        }
        else if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            _qrCode.gameObject.SetActive(true);            
            _myNetworkHUD.OnServerOnly();
            //_myNetworkHUD.TestOnStatClient();
        }
        else
        {
            // input form then
            //StartCoroutine(_myNetworkHUD.GetValidateUserToken(networkInfo.ExtractToken(), Application.absoluteURL));
        }
    }
}
