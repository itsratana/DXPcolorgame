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
    Vector3 lastVelocity;
    [SerializeField] private float veloForce;

    [Header("Velocity")]
    [SerializeField] private float forceMultiplier;
    [SerializeField] private float launchForce;

    [Header("Distance")]
    public float playerDistance;
    public Vector3 startPosition;

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
    //public List<Transform> duckColors = new List<Transform>();
    public PlayerColor currentColor = PlayerColor.red;
    public Color[] colorPlayer;
    public MeshRenderer playerMeshRenderer;

    [Header("Countdown")]
    private float checkInterval = 0.1f;
    private float afkCheck = 10f;
    private Coroutine countDownCoroutine;
    private bool isCoroutineStart = false;

    public enum PlayerColor
    {
        red, blue, green
    }
    /*public void ChangeColor(int colorID)
    {
        for (int i = 0; i < duckColors.Count; i++)
        {
            if (i == colorID)
            {
                duckColors[i].gameObject.SetActive(true);
                continue;
            }
            duckColors[i].gameObject.SetActive(false);
        }
    }*/
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
        RpcSetUpID(id);
    }

    [ClientRpc]
    public void RpcSetUpID(int id)
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
        if (isServer)
        {
            refManager.countdownTimeP1.gameObject.SetActive(false);
            refManager.countdownTimeP2.gameObject.SetActive(false);
        }

        Input.gyro.enabled = true;

        rb = GetComponent<Rigidbody>();
        lineRotation = GetComponent<Rotation>();
        switch (playerID)
        {
            case 0:
                trajectoryLine.gameObject.layer = 11;
                if (isLocalPlayer) refManager.identifierColor.color = new Color32(255, 0, 0, 255);

                currentColor = PlayerColor.red;
                playerMeshRenderer.material.color = Color.red;
                //duckColors[0].gameObject.SetActive(true);

                powerBarGO = refManager.powerBarGOP1;
                PowerBarMask = refManager.PowerBarMaskP1;

                break;
            case 1:
                trajectoryLine.gameObject.layer = 12;
                if (isLocalPlayer) refManager.identifierColor.color = new Color32(0, 0, 255, 255);

                currentColor = PlayerColor.blue;
                playerMeshRenderer.material.color = Color.blue;
                //duckColors[1].gameObject.SetActive(true);

                powerBarGO = refManager.powerBarGOP2;
                PowerBarMask = refManager.PowerBarMaskP2;

                break;
        }
    }

    void Update()
    {
        if (playerID == 0)
        {
            if (isServer)
            {
                if (VictoryPanel.instance.p1HasWon || GameOverPanel.instance.isGameOverP1)
                {
                    StopAllCoroutines();
                    powerBarGO.SetActive(false);
                    return;
                }
            }
        }
        else if (playerID == 1)
        {
            if (isServer)
            {
                if (VictoryPanel.instance.p2HasWon || GameOverPanel.instance.isGameOverP2)
                {
                    StopAllCoroutines();
                    powerBarGO.SetActive(false);
                    return;
                }
            }
        }
        PlayerMovement();
        Rotation();
        CalculateDistance();

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
                        if (countDownCoroutine != null)
                        {
                            StopCoroutine(countDownCoroutine);
                        }
                        countDownCoroutine = StartCoroutine(CheckVelocityAfk());

                        if (powerBarCoroutine != null)
                        {
                            StopCoroutine(powerBarCoroutine);
                        }
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
                playerReadyButton.GetComponent<Image>().color = new Color32(255, 0, 0, 255);
                playerReadyButton.GetComponentInChildren<TextMeshProUGUI>().text = "Cancel";
            }
            else
            {
                playerReadyButton.GetComponent<Image>().color = new Color32(0, 255, 0, 255);
                playerReadyButton.GetComponentInChildren<TextMeshProUGUI>().text = "Ready";
            }

        }
    }

    public override void OnStopClient()
    {
        playerReadyButton.GetComponent<Image>().color = new Color32(0, 255, 0, 255);
        playerReadyButton.GetComponentInChildren<TextMeshProUGUI>().text = "Ready";
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

    public float CalculateDistance()
    {
        return playerDistance = Vector3.Distance(startPosition, transform.position);
    }

    private void LateUpdate()
    {
        lastVelocity = rb.velocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 8)
        {
            float curSpeed = lastVelocity.magnitude;
            moveDirection = Vector3.Reflect(lastVelocity.normalized, collision.contacts[0].normal);
            rb.velocity = moveDirection * Mathf.Max(curSpeed, 0);
        }
        if (collision.gameObject.layer == 3)
        {
            float curSpeed = lastVelocity.magnitude;
            Vector3 forceDirection = collision.contacts[0].point - transform.position;
            forceDirection = -forceDirection.normalized;
            collision.gameObject.GetComponent<Rigidbody>().AddForce(forceDirection * curSpeed, ForceMode.Impulse);
            rb.AddForce(-forceDirection * curSpeed, ForceMode.Impulse);
        }
        if (collision.gameObject.layer == 10)
        {
            float curSpeed = lastVelocity.magnitude * 0.5f;
            moveDirection = Vector3.Reflect(lastVelocity.normalized, collision.contacts[0].normal);

            rb.velocity = moveDirection * Mathf.Max(curSpeed, 0);
        }
    }

    IEnumerator CheckVelocityAfk()
    {
        isCoroutineStart = true;

        float elapsedTime = 0f;

        yield return new WaitForSeconds(1);

        while (elapsedTime < afkCheck)
        {
            yield return new WaitForSeconds(checkInterval);

            if (rb.velocity.magnitude < 0.01f)
            {
                elapsedTime += checkInterval;
                if (elapsedTime >= 0)
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
                        refManager.countdownTimeP1.gameObject.SetActive(false);
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
            if (playerID == 0)
            {
                if (!VictoryPanel.instance.p1HasWon)
                {
                    GameOverPanel.instance.isGameOverP1 = true;
                }
            }
            if (playerID == 1)
            {
                if (!VictoryPanel.instance.p2HasWon)
                {
                    GameOverPanel.instance.isGameOverP2 = true;
                }
            }
            GameOverPanel.instance.GameOver();
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
