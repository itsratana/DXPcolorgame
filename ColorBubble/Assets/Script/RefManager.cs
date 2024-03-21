using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RefManager : MonoBehaviour
{
    public static RefManager Instance { get; private set; }

    [Header("Camera")]
    public CameraFollow cameraP1;
    public CameraFollow cameraP2;

    [Header("AFK Countdown")]
    public TextMeshProUGUI countdownTimeP1;
    public TextMeshProUGUI countdownTimeP2;

    [Header("Score")]

    public RectTransform networkInputField;
    public Button launchButton;

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
