using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;
using Mirror;

public class PlayerII : NetworkBehaviour
{
    public DataSaveManager dataSaveManager;
    public RefManager refManager;

    //Player
    //private bool isDead = false;
    private Rigidbody rb;
    [SerializeField] float horizontalSpeed;
    private float curSpeed;
    private int Bounce = 0;
    Vector3 lastVelocity;
    [SerializeField] private float veloForce;

    private Rotation lineRotation;
    private float accumulatedRotation = 0f;
    public float rotationSpeed = 100f;
    private float rotationAmount;
    Vector3 acceleration;
    float horizontalInput;

    //Audio Input
    public AudioLoudnessDetection detector;
    public float loudnessSensibility;
    public float threshold;
    public float thresholdMove;

    private Vector3 moveDirection;
    [SerializeField] TrajectoryLine trajectoryLine;

    [SerializeField] float loudness;
    [SerializeField] float syncedloudness;
    Vector3 playerMovement;

    //Color
    public PlayerColor currentColor = PlayerColor.red;
    public Color[] colorPlayer;

    //Inactive Countdown
    private float checkInterval = 0.1f;
    private float afkCheck = 11f;
    public TextMeshProUGUI countdownText;
    private Coroutine countDownCoroutine;
    private bool isCoroutineStart = false;

    /*private void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 200, 100), "Click"))
        {
            LoadSettings();
        }
    }*/

    public enum PlayerColor
    {
        red, blue, green
    }
    public void ChangeColor(Color newColor)
    {
        GetComponent<Renderer>().material.color = newColor;
    }

    public void ChangePlayerColor(PlayerColor newPlayerColor)
    {
        currentColor = newPlayerColor;
    }
    private void Awake()
    {
        syncDirection = SyncDirection.ClientToServer;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        lineRotation = GetComponent<Rotation>();
        LoadSettings();

        countdownText.gameObject.SetActive(false);
            
        
        //countDownCoroutine = StartCoroutine(CheckVelocityAfk());
    }

    void Update()
    {
        PlayerMovement();
        Rotation();
    }
    private void PlayerMovement()
    {
        playerMovement = transform.forward;
        if (isLocalPlayer)
        {
            loudness = detector.GetLoudnessFromMicrophone() * loudnessSensibility;

            if(loudness != syncedloudness)
            {
                syncedloudness = loudness;
                CmdLoudnessChanged(syncedloudness);
            }
        }
        if (isServer)
        {
            if (loudness > threshold)
            {
                if (rb.velocity.magnitude < thresholdMove)
                {
                    rb.AddForce(playerMovement * loudness * veloForce, ForceMode.Impulse);
                    //StopCoroutine(countDownCoroutine);
                    isCoroutineStart = false;
                    countdownText.gameObject.SetActive(false);
                }
            }
            if (rb.velocity.magnitude < thresholdMove)
            {
                trajectoryLine.ShowLine(true);
                if (!isCoroutineStart)
                {
                    //countDownCoroutine = StartCoroutine(CheckVelocityAfk());
                }
            }
            else
            {
                trajectoryLine.ShowLine(false);
            }

            if (transform.position.y < -20f)
            {
                Scene currentScene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(currentScene.name);
            }
        }   
    }
    private void Rotation()
    {
        accumulatedRotation = Mathf.Clamp(accumulatedRotation, -30f, 30f);
        transform.rotation = Quaternion.Euler(0f, accumulatedRotation, 0f);

        if (isLocalPlayer)
        {
            horizontalInput = Input.GetAxis("Horizontal");
            // acceleration = Input.acceleration;
            CmdInputChanged(horizontalInput);
        }
        if(isServer)
        {
            rotationAmount = horizontalInput * rotationSpeed * Time.deltaTime;
            accumulatedRotation += rotationAmount;

            /*float accelerationRotation = acceleration.x * rotationSpeed * Time.deltaTime;
            accumulatedRotation += accelerationRotation;*/
        }
        
    }

    [Command(channel = Channels.Unreliable)]
    void CmdLoudnessChanged(float loudness)
    {
        this.loudness = loudness;
    }
    [Command(channel = Channels.Unreliable)]
    void CmdInputChanged(float input)
    {
        horizontalInput = input;
    }
    public void UpdateThreshold(float newThreshold)
    {
        threshold = newThreshold;
    }
    public void UpdateLoudnessSen(float newLoudnessSen)
    {
        loudnessSensibility = newLoudnessSen;
    }
    public void UpdateRotationSen(float newRotationSpeed)
    {
        rotationSpeed = newRotationSpeed;
    }
    public SettingsData GetSettingsData()
    {
        return new SettingsData
        {
            Threshold = threshold,
            LoudnessSen = loudnessSensibility,
            RotationSen = rotationSpeed,
        };
    }
    public void ApplySettings(SettingsData settingsData)
    {
        threshold = settingsData.Threshold;
        loudnessSensibility = settingsData.LoudnessSen;
        rotationSpeed = settingsData.RotationSen;
    }
    private void LoadSettings()
    {
        SettingsData settingsData = dataSaveManager.LoadData();
        ApplySettings(settingsData);
    }

    private void LateUpdate()
    {
        lastVelocity = rb.velocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 8)
        {
            curSpeed = lastVelocity.magnitude;
            moveDirection = Vector3.Reflect(lastVelocity.normalized, collision.contacts[0].normal);

            rb.velocity = moveDirection * Mathf.Max(curSpeed, 0);
            Bounce++;
        }
    }

    /*public void SetIsDead(bool value)
    {
        isDead = true;
    }

    public bool GetIsDead()
    {
        return isDead;
    }*/

    IEnumerator CheckVelocityAfk()
    {
        isCoroutineStart = true;
        //yield return new WaitForSeconds(1f);

        float elapsedTime = 0f;
        //countdownText.gameObject.SetActive(true);

        while (elapsedTime < afkCheck)
        {
            yield return new WaitForSeconds(checkInterval);

            if (rb.velocity.magnitude < 0.01f)
            {
                elapsedTime += checkInterval;
                if (elapsedTime >= 1)
                {
                    countdownText.gameObject.SetActive(true);
                }
            }
            else
            {
                elapsedTime = 0f;
                //break;
            }
            UpdateCountdownText(afkCheck - elapsedTime);
        }
        if (elapsedTime >= afkCheck)
        {
            //SetIsDead(true);
            GameOverP2.instance.GameOver();
            GameOverP2.instance.isGameOver = true;
            Debug.Log("Inactive for 10 seconds!");
        }
        isCoroutineStart = false;
    }
    void UpdateCountdownText(float timeRemaining)
    {
        //countdownText.gameObject.SetActive(!countdownText.gameObject.activeSelf);
        //if (rb.velocity.magnitude < 0.01f)
        //{
        //    countdownText.gameObject.SetActive(true);
        //}
        //else
        //{
        //    countdownText.gameObject.SetActive(false);
        //}
        countdownText.text = Mathf.CeilToInt(timeRemaining).ToString("0");
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        refManager = FindObjectOfType<RefManager>();
        if (refManager != null)
        {
            dataSaveManager = refManager.dataSaveManager;
            countdownText = refManager.countdownTime;
            refManager.virtualCamera.Follow = transform;

            if (isLocalPlayer)
            {
                CmdSetupRefManager();
            }
        }
    }
    [Command]
    private void CmdSetupRefManager()
    {
        refManager = FindObjectOfType<RefManager>();
        if (refManager != null)
        {
            dataSaveManager = refManager.dataSaveManager;
            countdownText = refManager.countdownTime;
            refManager.virtualCamera.Follow = transform;
        }
    }
}
