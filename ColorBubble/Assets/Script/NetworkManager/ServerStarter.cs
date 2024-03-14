using UnityEngine;

public class ServerStarter : MonoBehaviour
{
    [SerializeField] private MyNetworkHUD _myNetworkHUD = null;
    [SerializeField] private Transform _titleGame = null;
    [SerializeField] private Transform _qrCode = null;
    [SerializeField] private NetworkInfo networkInfo = null;

    private void Start()
    {
        StartServer();
    }

    public void StartServer()
    {
        _titleGame.gameObject.SetActive(true);
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
            //_myNetworkHUD.TestOnStatClient();
            _myNetworkHUD.OnServerOnly();
        }
        else
        {
            // input form then
            StartCoroutine(_myNetworkHUD.GetValidateUserToken(networkInfo.ExtractToken(), Application.absoluteURL));
        }
    }
}
