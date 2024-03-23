using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using Mirror;

public class PlayerII : NetworkBehaviour
{
    public int playerID;

    //public CameraFollow cameraTransform;

    MyNetworkManager myNetworkManager = (MyNetworkManager)NetworkManager.singleton;
    public RefManager refManager;
    private Rigidbody rb;
    [SerializeField] float horizontalSpeed;
    private float curSpeed;
    Vector3 lastVelocity;
    [SerializeField] private float veloForce;

    [Header("Velocity")]
    [SerializeField] private float forceMultiplier;
    [SerializeField] private float launchForce;

    [Header("Button")]
    public Button launchButton;
    private bool isLaunchPressed = false;
    public Button playerReadyButton;
    public bool isReady = false;

    [Header("PowerBar")]
    public GameObject powerBarGO;
    public Image PowerBarMask;
    public float barChangeSpeed = 1;
    private readonly float maxPowerBarValue = 100;
    float currentPowerBarValue = 0;
    bool powerIsIncreasing = true;
    private readonly bool PowerBarON = true;
    private Coroutine powerBarCoroutine;

    [Header("Rotation")]
    [SerializeField] TrajectoryLine trajectoryLine;
    public float rotationSpeed = 100f;
    private Rotation lineRotation;
    private float accumulatedRotation = 0f;
    private float rotationAmount;
    float horizontalInput;
    Vector3 acceleration;
    private Vector3 moveDirection;

    Vector3 playerMovement;

    [Header("Color")]
    public PlayerColor currentColor = PlayerColor.red;
    public Color[] colorPlayer;
    public MeshRenderer playerMeshRenderer;

    [Header("Countdown")]
    private float checkInterval = 0.1f;
    private float afkCheck = 11f;
    private Coroutine countDownCoroutine;
    private bool isCoroutineStart = false;

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

    [ServerCallback]
    public void SetUpID(int id)
    {
        playerID = id;
    }
    private void Awake()
    {
        syncDirection = SyncDirection.ClientToServer;
        refManager = FindObjectOfType<RefManager>();
    }

    void Start()
    {
        Input.gyro.enabled = true;

        rb = GetComponent<Rigidbody>();
        lineRotation = GetComponent<Rotation>();

        switch (playerID)
        {
            case 0:
                currentColor = PlayerColor.red;
                playerMeshRenderer.material.color = Color.red;

                powerBarGO = refManager.powerBarGOP1;
                PowerBarMask = refManager.PowerBarMaskP1;

                break;
            case 1:
                currentColor = PlayerColor.blue;
                playerMeshRenderer.material.color = Color.blue;

                powerBarGO = refManager.powerBarGOP2;
                PowerBarMask = refManager.PowerBarMaskP2;

                break;
        }
    }

    void Update()
    {
        PlayerMovement();
        Rotation();

        if (isServer)
        {
            if (myNetworkManager.isGameStart)
            {
                Timer.instance.TimerCountdown();
            }
        }
    }
    private void PlayerMovement()
    {
        launchForce = currentPowerBarValue * forceMultiplier;
        playerMovement = transform.forward;

        if (isServer)
        {
            if (myNetworkManager.isGameStart)
            {
                if (isLaunchPressed)
                {
                    isLaunchPressed = false;
                    if (rb.velocity.magnitude < 0.01f)
                    {
                        rb.AddForce(playerMovement * launchForce, ForceMode.Impulse);
                        StopCoroutine(countDownCoroutine);
                        StopCoroutine(powerBarCoroutine);
                        isCoroutineStart = false;
                        if (playerID == 0)
                        {
                            refManager.countdownTimeP1.gameObject.SetActive(false);
                        }
                        else if (playerID == 1)
                        {
                            refManager.countdownTimeP2.gameObject.SetActive(false);
                        }
                    }
                }
                if (rb.velocity.magnitude < 0.01f)
                {
                    powerBarGO.SetActive(true);
                    trajectoryLine.ShowLine(true);
                    if (!isCoroutineStart)
                    {
                        countDownCoroutine = StartCoroutine(CheckVelocityAfk());
                        powerBarCoroutine = StartCoroutine(UpdatePowerBar());
                    }
                }
                else
                {
                    powerBarGO.SetActive(false);
                    trajectoryLine.ShowLine(false);
                }
            }
        }
    }
    [Command]
    public void CmdOnClickLaunch()
    {
        isLaunchPressed = true;
    }
    [Command]
    public void CmdPlayerReady(bool isReady)
    {
        this.isReady = isReady;
        myNetworkManager.PlayerReadyCheck();
    }
    public void PlayerReady()
    {
        isReady = !isReady;
        CmdPlayerReady(isReady);
        if (isLocalPlayer)
        {
            if (isReady)
            {
                playerReadyButton.GetComponent<Image>().color = new Color32(238, 91, 97, 255);
            }
            else
            {
                playerReadyButton.GetComponent<Image>().color = new Color32(91, 238, 109, 255);
            }
        }
    }
    IEnumerator UpdatePowerBar()
    {
        while (PowerBarON)
        {
            if (!powerIsIncreasing)
            {
                currentPowerBarValue -= barChangeSpeed;
                if (currentPowerBarValue <= 0)
                {
                    powerIsIncreasing = true;
                }
            }
            if (powerIsIncreasing)
            {
                currentPowerBarValue += barChangeSpeed;
                if (currentPowerBarValue >= maxPowerBarValue)
                {
                    powerIsIncreasing = false;
                }
            }
            float fill = currentPowerBarValue / maxPowerBarValue;
            PowerBarMask.fillAmount = fill;
            yield return new WaitForSeconds(0.02f);
        }
        yield return null;
    }
    private void Rotation()
    {
        accumulatedRotation = Mathf.Clamp(accumulatedRotation, -30f, 30f);
        transform.rotation = Quaternion.Euler(0f, accumulatedRotation, 0f);

        if (isLocalPlayer)
        {
            acceleration = Input.acceleration;
            CmdRotationChanged(acceleration.x);

            horizontalInput = Input.GetAxis("Horizontal");
            CmdHorizontalChanged(horizontalInput);
        }
        if (isServer)
        {
            float accelerationRotation = acceleration.x * rotationSpeed * Time.deltaTime;
            accumulatedRotation += accelerationRotation;

            rotationAmount = horizontalInput * rotationSpeed * Time.deltaTime;
            accumulatedRotation += rotationAmount;
        }

    }
    [Command(channel = Channels.Unreliable)]
    void CmdRotationChanged(float input)
    {
        acceleration.x = input;
    }
    [Command(channel = Channels.Unreliable)]
    void CmdHorizontalChanged(float input)
    {
        horizontalInput = input;
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
            //Bounce++;
        }
        if (collision.gameObject.layer == 3)
        {
            Vector3 forceDirection = collision.contacts[0].point - transform.position;
            forceDirection = -forceDirection.normalized;
            collision.gameObject.GetComponent<Rigidbody>().AddForce(forceDirection * curSpeed, ForceMode.Impulse);
            rb.AddForce(-forceDirection * curSpeed, ForceMode.Impulse);
        }
    }

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
                    if (playerID == 0)
                    {
                        refManager.countdownTimeP1.gameObject.SetActive(true);
                    }
                    else if (playerID == 1)
                    {
                        refManager.countdownTimeP2.gameObject.SetActive(true);
                    }
                }
                else if (elapsedTime > 10)
                {
                    if (playerID == 0)
                    {
                        refManager.countdownTimeP2.gameObject.SetActive(false);
                    }
                    else if (playerID == 1)
                    {
                        refManager.countdownTimeP2.gameObject.SetActive(false);
                    }
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
        if (playerID == 0)
        {
            refManager.countdownTimeP1.text = Mathf.CeilToInt(timeRemaining).ToString("0");
        }
        else if (playerID == 1)
        {
            refManager.countdownTimeP2.text = Mathf.CeilToInt(timeRemaining).ToString("0");
        }
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (refManager != null)
        {
            //cameraTransform = refManager.cameraP1;
            //cameraTransform = refManager.cameraP2;
            if (isLocalPlayer)
            {
                launchButton = RefManager.Instance.launchButton;
                launchButton.onClick.AddListener(CmdOnClickLaunch);

                playerReadyButton = refManager.readyButton;
                playerReadyButton.onClick.AddListener(PlayerReady);
                //CmdSetupRefManager();
            }
        }
    }
    [ClientRpc]
    public void GameStart()
    {
        if (isLocalPlayer)
        {
            if (RefManager.Instance.launchButton)
            {
                RefManager.Instance.launchButton.gameObject.SetActive(true);
            }
            if (RefManager.Instance.readyButton)
            {
                RefManager.Instance.readyButton.gameObject.SetActive(false);
            }
        }
    }

    /*    [Command]
        private void CmdSetupRefManager()
        {
            if (refManager != null)
                refManager.virtualCamera.Follow = transform;
        }*/
}
