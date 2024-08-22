using UnityEngine;
using UnityEngine.UIElements;

public class SettingsHandler : MonoBehaviour
{
    public static SettingsHandler Instance { get; private set; }

    private UIDocument uiDocument;
    private static Slider sensitivitySlider;
    private static Slider volumeSlider;
    private static Toggle fullscreenToggle;

    private const string SensitivityKey = "Sensitivity";
    private const string VolumeKey = "Volume";
    private const string FullscreenKey = "Fullscreen";

    public static float Sensitivity { get; private set; }
    public static float Volume { get; private set; }
    public static bool Fullscreen { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        uiDocument = GetComponent<UIDocument>();
        if (uiDocument == null)
        {
            Debug.LogError("UIDocument component not found!");
            return;
        }

        InitializeUIElements();
        LoadSettings();
        AddEventListeners();
    }

    private void InitializeUIElements()
    {
        sensitivitySlider = uiDocument.rootVisualElement.Q<Slider>("sensitivity-slider");
        volumeSlider = uiDocument.rootVisualElement.Q<Slider>("volume-slider");
        fullscreenToggle = uiDocument.rootVisualElement.Q<Toggle>("fullscreen-toggle");
    }

    private void LoadSettings()
    {
        Sensitivity = PlayerPrefs.GetFloat(SensitivityKey, 500f);
        Volume = PlayerPrefs.GetFloat(VolumeKey, 0.75f);
        Fullscreen = PlayerPrefs.GetInt(FullscreenKey, 1) == 1;

        sensitivitySlider.value = Sensitivity;
        volumeSlider.value = Volume;
        fullscreenToggle.value = Fullscreen;

        ApplySettings();
    }

    private void AddEventListeners()
    {
        sensitivitySlider.RegisterValueChangedCallback(OnSensitivityChanged);
        volumeSlider.RegisterValueChangedCallback(OnVolumeChanged);
        fullscreenToggle.RegisterValueChangedCallback(OnFullscreenChanged);
    }

    private void OnSensitivityChanged(ChangeEvent<float> evt)
    {
        Sensitivity = evt.newValue;
        PlayerPrefs.SetFloat(SensitivityKey, Sensitivity);
        PlayerPrefs.Save();
        // Apply sensitivity change to your game logic here
    }

    private void OnVolumeChanged(ChangeEvent<float> evt)
    {
        Volume = evt.newValue;
        PlayerPrefs.SetFloat(VolumeKey, Volume);
        PlayerPrefs.Save();
        AudioListener.volume = Volume;
    }

    private void OnFullscreenChanged(ChangeEvent<bool> evt)
    {
        Fullscreen = evt.newValue;
        PlayerPrefs.SetInt(FullscreenKey, Fullscreen ? 1 : 0);
        PlayerPrefs.Save();
        Screen.fullScreen = Fullscreen;
    }

    private void ApplySettings()
    {
        // Apply sensitivity to your game logic here
        AudioListener.volume = Volume;
        Screen.fullScreen = Fullscreen;
    }

    public static void SaveSettings()
    {
        PlayerPrefs.SetFloat(SensitivityKey, Sensitivity);
        PlayerPrefs.SetFloat(VolumeKey, Volume);
        PlayerPrefs.SetInt(FullscreenKey, Fullscreen ? 1 : 0);
        PlayerPrefs.Save();
    }
}