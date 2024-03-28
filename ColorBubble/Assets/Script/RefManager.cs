using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RefManager : MonoBehaviour
{
    public static RefManager Instance { get; private set; }

    public List<Transform> playerStartPositions = new List<Transform>();

    [Header("Camera")]
    public CameraFollow cameraP1;
    public CameraFollow cameraP2;

    [Header("AFK Countdown")]
    public TextMeshProUGUI countdownTimeP1;
    public TextMeshProUGUI countdownTimeP2;

    [Header("PowerBar")]
    public GameObject powerBarGOP1;
    public Image PowerBarMaskP1;
    public GameObject powerBarGOP2;
    public Image PowerBarMaskP2;

    public RectTransform networkInputField;
    public Button launchButton;
    public Button readyButton;

    public TextMeshProUGUI startingGameCountdown;

    private void Awake()
    {
        // Check if an instance already exists
        if (Instance == null)
        {
            // If not, set this instance as the singleton instance
            Instance = this;
        }
        else
        {
            // If an instance already exists, destroy this instance
            Debug.LogWarning("Multiple instances of YourClass found. Destroying this instance.");
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        // Ensure that the instance is nullified when destroyed
        if (Instance == this)
        {
            Instance = null;
        }
    }
}
