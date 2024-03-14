using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : MonoBehaviour
{
    public GameObject menuPanel;
    public GameObject confirmationPanel;
    public GameObject confirmSavePanel;

    public Slider thresholdSlider;
    public Slider loudnessSenSlider;
    public Slider rotationSenSlider;

    public PlayerII player;
    //public Rotation rotation;
    public DataSaveManager dataSaveManager;

    public float defaultThreshold = 18f; 
    public float defaultLoudnessSen = 69f;
    public float defaultRotationSen = 100f;

    private float currentThreshold;
    private float currentLoudnessSen;
    private float currentRotationSen;

    private bool settingChanged = false;

    public static SettingPanel Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        player = FindObjectOfType<PlayerII>();
    }
    void Start()
    {
        
        //rotation = FindObjectOfType<Rotation>();
        dataSaveManager = FindObjectOfType<DataSaveManager>();

        LoadSettings();

        thresholdSlider.onValueChanged.AddListener(OnThresholdValueChanged);
        loudnessSenSlider.onValueChanged.AddListener(OnLoudnessSenChanged);   
        rotationSenSlider.onValueChanged.AddListener (OnRotationSenChanged);

        confirmationPanel.SetActive(false);
        confirmSavePanel.SetActive(false);
    }   

    public void OnThresholdValueChanged(float newValue)
    {
        player.CmdThreshold(newValue);
        settingChanged = true;
    }
    public void OnRotationSenChanged(float newValue)
    {
        player.CmdRotationSen(newValue);
        settingChanged = true;
    }
    public void OnLoudnessSenChanged(float newValue)
    {
        player.CmdLoudnessSen(newValue);
        settingChanged = true;
    }
    public void ResetToDefault()
    {
        confirmationPanel.SetActive(true);
        gameObject.SetActive(false);
    }
    public void ConfirmReset()
    {
        /*currentThreshold = defaultThreshold;
        currentLoudnessSen = defaultLoudnessSen;
        currentRotationSen = defaultRotationSen;*/

        thresholdSlider.value = defaultThreshold;
        loudnessSenSlider.value = defaultLoudnessSen;
        rotationSenSlider.value = defaultRotationSen;

        confirmationPanel.SetActive(false);
        gameObject.SetActive(true);
    }
    public void CancelReset()
    {
        confirmationPanel.SetActive(false);
        gameObject.SetActive(true);

    }
    public void CloseSettings()
    {
        if (settingChanged)
        {
            confirmSavePanel.SetActive(true);
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(false);
            menuPanel.SetActive(true);
        }
    }
    public void ConfirmSaveSetting()
    {
        currentThreshold = thresholdSlider.value;
        currentLoudnessSen = loudnessSenSlider.value;
        currentRotationSen = rotationSenSlider.value;

        confirmSavePanel.SetActive(false);
        gameObject.SetActive(false);
        menuPanel.SetActive(true);

        SaveSettings();
    }
    public void DiscardSaveSetting()
    {
        confirmSavePanel.SetActive(false);
        gameObject.SetActive(false);
        menuPanel.SetActive(true);

        thresholdSlider.value = (currentThreshold != 0) ? currentThreshold : defaultThreshold;
        loudnessSenSlider.value = (currentLoudnessSen != 0) ? currentLoudnessSen : defaultLoudnessSen;
        rotationSenSlider.value = (currentRotationSen != 0) ? currentRotationSen : defaultRotationSen;
    }
    public void SaveSettingBtn()
    {
        currentThreshold = thresholdSlider.value;
        currentLoudnessSen = loudnessSenSlider.value;
        currentRotationSen = rotationSenSlider.value;

        SaveSettings();
    }
    private void SaveSettings()
    {
        SettingsData settingsData = new SettingsData
        {
            Threshold = thresholdSlider.value,
            LoudnessSen = loudnessSenSlider.value,
            RotationSen = rotationSenSlider.value,
        };
        dataSaveManager.SaveData(settingsData);
        settingChanged = false;
    }
    private void LoadSettings()
    {
        SettingsData settingsData = dataSaveManager.LoadData();

        if (settingsData != null)
        {
            thresholdSlider.value = settingsData.Threshold;
            loudnessSenSlider.value = settingsData.LoudnessSen;
            rotationSenSlider.value = settingsData.RotationSen;

            thresholdSlider.GetComponent<SliderPercent>().UpdateText();
            rotationSenSlider.GetComponent<SliderPercent>().UpdateText();
            loudnessSenSlider.GetComponent<SliderPercent>().UpdateText();

            player.ApplySettings(settingsData);
            //rotation.ApplySettings(settingsData);
        }
        else
        {
            thresholdSlider.value = defaultThreshold;
            loudnessSenSlider.value = defaultLoudnessSen;
            rotationSenSlider.value = defaultRotationSen;
        }
        settingChanged = false;
    }
}
[System.Serializable]
public class SettingsData
{
    public float Threshold;
    public float LoudnessSen;
    public float RotationSen;
}
