using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using Mirror;

public class PlayerII : NetworkBehaviour
{
    public RefManager refManager;

    //Player
    //private bool isDead = false;
    private Rigidbody rb;
    [SerializeField] float horizontalSpeed;
    private float curSpeed;
    private int Bounce = 0;
    Vector3 lastVelocity;
    [SerializeField] private float veloForce;

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
        //LoadSettings();

        countdownText.gameObject.SetActive(false);
        Input.gyro.enabled = true;    
        
        countDownCoroutine = StartCoroutine(CheckVelocityAfk());
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

    /*public void UpdateThreshold(float newThreshold)
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
    }*/

    [Command]
    public void CmdRotationSen(float newRotationSpeed)
    {
        rotationSpeed = newRotationSpeed;
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
