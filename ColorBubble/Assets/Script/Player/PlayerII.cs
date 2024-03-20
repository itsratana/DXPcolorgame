using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using Mirror;

public class PlayerII : NetworkBehaviour
{
    public RefManager refManager;
    private Rigidbody rb;
    [SerializeField] float horizontalSpeed;
    private float curSpeed;
    private int Bounce = 0;
    Vector3 lastVelocity;
    [SerializeField] private float veloForce;

    [Header("PowerBar")]
    public GameObject powerBarGO;
    public Image PowerBarMask;
    public float barChangeSpeed = 1;
    float maxPowerBarValue = 100;
    float currentPowerBarValue = 0;
    bool powerIsIncreasing = true;
    bool PowerBarON = true;

    [Header("Rotation")]
    private Rotation lineRotation;
    private float accumulatedRotation = 0f;
    public float rotationSpeed = 100f;
    private float rotationAmount;
    float horizontalInput;
    Vector3 acceleration;
    private Vector3 moveDirection;
    [SerializeField] TrajectoryLine trajectoryLine;

    Vector3 playerMovement;

    [Header("Color")]
    public PlayerColor currentColor = PlayerColor.red;
    public Color[] colorPlayer;

    [Header("Countdown")]
    private float checkInterval = 0.1f;
    private float afkCheck = 11f;
    public TextMeshProUGUI countdownText;
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
    private void Awake()
    {
        syncDirection = SyncDirection.ClientToServer;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        lineRotation = GetComponent<Rotation>();
        //LoadSettings();

        countdownText.gameObject.SetActive(false);
        Input.gyro.enabled = true;    
        
        countDownCoroutine = StartCoroutine(CheckVelocityAfk());
        StartCoroutine(UpdatePowerBar());
    }

    void Update()
    {
        PlayerMovement();
        Rotation();
        Timer.instance.TimerCountdown();
    }
    private void PlayerMovement()
    {

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

            if (Input.touchCount > 0)
            {
                PowerBarON = false;
                //LaunchRocket();
                StartCoroutine(TurnOffPowerBar());

            }
        }
        yield return null;
    }
    IEnumerator TurnOffPowerBar()
    {
        yield return new WaitForSeconds(0.5f);
        powerBarGO.SetActive(false);
    }
    private void Rotation()
    {
        accumulatedRotation = Mathf.Clamp(accumulatedRotation, -30f, 30f);
        transform.rotation = Quaternion.Euler(0f, accumulatedRotation, 0f);

        if (isLocalPlayer)
        {
           // horizontalInput = Input.GetAxis("Horizontal");
            acceleration = Input.acceleration;
           // CmdInputChanged(horizontalInput);
            CmdRotationChanged(acceleration.x);
        }
        if(isServer)
        {
            rotationAmount = horizontalInput * rotationSpeed * Time.deltaTime;
            accumulatedRotation += rotationAmount;

            float accelerationRotation = acceleration.x * rotationSpeed * Time.deltaTime;
            accumulatedRotation += accelerationRotation;
        }
        
    }
    [Command(channel = Channels.Unreliable)]
    void CmdRotationChanged(float input)
    {
        acceleration.x = input;
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
        countdownText.text = Mathf.CeilToInt(timeRemaining).ToString("0");
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        refManager = FindObjectOfType<RefManager>();
        if (refManager != null)
        {
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
            countdownText = refManager.countdownTime;
            refManager.virtualCamera.Follow = transform;
        }
    }
    /*private void PlayerMovement()
    {
        playerMovement = transform.forward;
        if (isLocalPlayer)
        {
           
        }
        if (isServer)
        {
            if (loudness > threshold)
            {
                if (rb.velocity.magnitude < thresholdMove)
                {
                    rb.AddForce(playerMovement * loudness, ForceMode.Impulse);
                    StopCoroutine(countDownCoroutine);
                    isCoroutineStart = false;
                    countdownText.gameObject.SetActive(false);
                }
            }
            if (rb.velocity.magnitude < thresholdMove)
            {
                trajectoryLine.ShowLine(true);
                if (!isCoroutineStart)
                {
                    countDownCoroutine = StartCoroutine(CheckVelocityAfk());
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
    }*/
}
