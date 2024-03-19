using UnityEngine.UI;
using UnityEngine;

public class PowerBar : MonoBehaviour
{
    public static PowerBar instance { get; private set; }

    [Header("Power Bar")]
    public GameObject powerBarGO;
    public Image PowerBarMask;
    public float barChangeSpeed = 1;
    float maxPowerBarValue = 100;
    float currentPowerBarValue;
    bool powerIsIncreasing;
    bool PowerBarON;


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
