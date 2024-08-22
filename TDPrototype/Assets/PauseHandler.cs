
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Rendering.PostProcessing;

public class PauseHandler : MonoBehaviour
{
    PostProcessVolume vol;
    bool transitionInto;
    Button pauseButton;
    VisualElement pausemenucontainer;
    [SerializeField]GameObject pauseScreen;

    void Start()
    {
        vol = GetComponent<PostProcessVolume>();

        pausemenucontainer = pauseScreen.GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("pauseScreenEle");

        // Get the UI Document and root VisualElement
        var uiDocument = GetComponent<UIDocument>();
        var root = uiDocument.rootVisualElement;

        // Find the button by ID
        pauseButton = root.Q<Button>("pausebutton");
        if (pauseButton != null)
        {
            // Register the button click event
            pauseButton.clicked += OnPauseButtonClicked;
        }

        root = pauseScreen.GetComponent<UIDocument>().rootVisualElement;

        // Find and configure the button
        Button myButton = root.Q<Button>("resume-button");
        myButton.clickable.clicked += OnResumeButton;

        myButton = root.Q<Button>("settings-button");
        myButton.clickable.clicked += OnSettingsButton;

        myButton = root.Q<Button>("back-settings-button");
        myButton.clickable.clicked += OnSettingsButton;

    }

    void Update()
    {
        if (transitionInto)
        {
            vol.weight = Mathf.Lerp(vol.weight, 1f, Time.unscaledDeltaTime*4f);
        }
        else
        {
            vol.weight = Mathf.Lerp(vol.weight, 0f, Time.unscaledDeltaTime*4f);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pausemenucontainer.ClassListContains("hidden"))
            {
                OnPauseButtonClicked();
            }
            else
            {
                OnResumeButton();
            }
            
        }
    }

    private void OnPauseButtonClicked()
    {
        Time.timeScale = 0f;
        
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible = true;

        transitionInto = !transitionInto;

        
        pausemenucontainer.RemoveFromClassList("hidden");
    }



    private void OnResumeButton()
    {
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
        pausemenucontainer.Q<VisualElement>("pauseScreenEle").AddToClassList("hidden");
        pauseScreen.GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("settingsScreenEle").AddToClassList("hidden2");
        transitionInto = !transitionInto;
    }

    private void OnEndRoundButton()
    {
        PlayerMovement.SwitchPerspective();
        OnResumeButton();
    }


    private void OnSettingsButton()
    {
        if (pauseScreen.GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("settingsScreenEle").ClassListContains("hidden2"))
        {
            Debug.Log("Opening");
            pauseScreen.GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("settingsScreenEle").RemoveFromClassList("hidden2");
        }
        else
        {
            Debug.Log("Closing");
            pauseScreen.GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("settingsScreenEle").AddToClassList("hidden2");
        }
        

    }
}
