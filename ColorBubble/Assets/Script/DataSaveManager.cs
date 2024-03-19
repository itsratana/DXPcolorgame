/*using UnityEngine;
using System.IO;

public class DataSaveManager : MonoBehaviour
{
    private string dataFileName = "savedData.json";
    private string filePath;

    private void Awake()
    {
        filePath = Path.Combine(Application.persistentDataPath, dataFileName);
    }

    public void SaveData(SettingsData data)
    {
        string jsonData = JsonUtility.ToJson(data);
        File.WriteAllText(filePath, jsonData);
    }

    public SettingsData LoadData()
    {
        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            return JsonUtility.FromJson<SettingsData>(jsonData);
        }
        else
        {
            SettingsData defaultSettings = new SettingsData
            {
                Threshold = SettingPanel.Instance.defaultThreshold,
                LoudnessSen = SettingPanel.Instance.defaultLoudnessSen,
                RotationSen = SettingPanel.Instance.defaultRotationSen
            };
            SaveData(defaultSettings);

            return defaultSettings;
        }
    }
}
*/