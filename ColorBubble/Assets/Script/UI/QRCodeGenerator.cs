using UnityEngine;
using UnityEngine.UI;
using ZXing;
using ZXing.QrCode;

public class QrCodeGenerator : MonoBehaviour
{
    [SerializeField] private RawImage _rawImageReceiver;
    [SerializeField] private NetworkInfo _networkInfo;

    private Texture2D _storeEncodedTexture;

    private void Start()
    {
        _storeEncodedTexture = new Texture2D(256, 256);
        if (Application.platform != RuntimePlatform.WebGLPlayer)
        {
            EncodeTextToQRode();
        }
    }

    private Color32[] Encode(string textForEncode, int width, int height)
    {
        BarcodeWriter writer = new BarcodeWriter
        {
            Format = BarcodeFormat.QR_CODE,
            Options = new QrCodeEncodingOptions
            {
                Height = height,
                Width = width,
            }
        };
        return writer.Write(textForEncode);
    }

    private void EncodeTextToQRode()
    {
        string ipAddress = _networkInfo.GetLocalIPAddress();
        Color32[] _convertPixelToTexture = Encode($"https://{ipAddress}/", _storeEncodedTexture.width, _storeEncodedTexture.height);
        //Color32[] _convertPixelToTexture = Encode($"https://{ipAddress}/heart/?ip={ipAddress}", _storeEncodedTexture.width, _storeEncodedTexture.height);
        //Color32[] _convertPixelToTexture = Encode($"https://games.direxplaylab.com/heart/?ip={ipAddress}", _storeEncodedTexture.width, _storeEncodedTexture.height);
        //Color32[] _convertPixelToTexture = Encode($"https://{ipAddress}", _storeEncodedTexture.width, _storeEncodedTexture.height);
        //Color32[] _convertPixelToTexture = Encode($"https://192.168.11.106/{ipAddress}", _storeEncodedTexture.width, _storeEncodedTexture.height);
        _storeEncodedTexture.SetPixels32(_convertPixelToTexture);
        _storeEncodedTexture.Apply();
        _rawImageReceiver.texture = _storeEncodedTexture;
    }
}