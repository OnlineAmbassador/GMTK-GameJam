using UnityEngine;
using UnityEngine.UIElements;

public class UIButtonSound : MonoBehaviour
{
    public AudioClip hoverSound;
    public AudioClip clickSound;
    private AudioSource audioSource;

    void Awake()
    {
        // Initialize the AudioSource component
        audioSource = gameObject.AddComponent<AudioSource>();

        // Get the root VisualElement
        var root = GetComponent<UIDocument>().rootVisualElement;

        // Query all buttons in the UI
        var buttons = root.Query<Button>().ToList();
        

        // Register the mouse enter and click events for each button
        foreach (var button in buttons)
        {
            button.RegisterCallback<MouseEnterEvent>(evt => MouseEnter());
            button.RegisterCallback<ClickEvent>(evt => OnClick());
        }
    }

    void MouseEnter()
    {
        // Play the hover sound when the mouse enters the element
        PlaySound(hoverSound);
    }

    void OnClick()
    {
        PlaySound(clickSound);
    }

    void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
