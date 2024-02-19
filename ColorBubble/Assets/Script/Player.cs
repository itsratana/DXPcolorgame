using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private CharacterController characterController;

    private bool isDead = false;

    [SerializeField] float horizontalSpeed;
    [SerializeField] float verticalSpeed = 0f;
    private readonly float gravity = -8f;

    readonly float fallRange = -100f;
    private float scaleUp;
    private float scaleDown;
    private readonly float maxScaleDown = 0.3f;
    [SerializeField] float accelerationSpeed;
    [SerializeField] float deacceleration;

    public AudioSource source;
    public AudioLoudnessDetection detector;

    public float loudnessSensibility;
    public float threshold;
    public float maxForwardSpeed;
    [SerializeField] float velocity=0f;
    public PlayerColor currentColor = PlayerColor.red;

    public Color[] colorPlayer;

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

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 acceleration = Input.acceleration;
        Vector3 playerMovement = new (acceleration.x, 0, vertical);

        float loudness = detector.GetLoudnessFromMicrophone() * loudnessSensibility;
       //Debug.Log("Loudness: " + loudness);

        if (loudness > threshold) 
        {
            velocity += accelerationSpeed * loudness * Time.deltaTime;
        }
        else
        {
            velocity -= deacceleration * Time.deltaTime;
        }
        velocity = Mathf.Clamp(velocity,0,maxForwardSpeed);

        //Gravity
        if (characterController.isGrounded)
        {
            verticalSpeed = -0.1f;
        }
        else
        {
            verticalSpeed += gravity * Time.deltaTime;
        }

        if (transform.position.y < fallRange)
        {
            isDead = true;
        }

        playerMovement.y = verticalSpeed;
        characterController.Move(horizontalSpeed * Time.deltaTime * playerMovement);

        Vector3 forwardMovement = velocity * Time.deltaTime * transform.forward;
        characterController.Move(forwardMovement);
    }
    public void ScaleUp()
    {
        Transform sphereTransform = gameObject.transform;
        scaleUp = 0.1f;
        sphereTransform.localScale += new Vector3(scaleUp, scaleUp, scaleUp);
    }

    public void ScaleDown()
    {
        Transform sphereTransform = gameObject.transform;
        scaleDown = 0.1f;

        scaleDown = Mathf.Clamp(scaleDown, 0, maxScaleDown);

        Vector3 newScale = sphereTransform.localScale - new Vector3(scaleDown, scaleDown, scaleDown);

        newScale = new Vector3(
            Mathf.Max(newScale.x, maxScaleDown),
            Mathf.Max(newScale.y, maxScaleDown),
            Mathf.Max(newScale.z, maxScaleDown)
        );
        sphereTransform.localScale = newScale;
    }
    public void SetIsDead(bool value)
    {
        isDead = true;
    }

    public bool GetIsDead()
    {
        return isDead;
    }
}

