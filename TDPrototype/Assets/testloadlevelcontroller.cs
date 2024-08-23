using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private void Start()
    {
        // Get the UIDocument component on the GameObject this script is attached to
        var uiDocument = GetComponent<UIDocument>();
        if (uiDocument == null)
        {
            Debug.LogError("UIDocument component not found on this GameObject.");
            return;
        }

        // Get the root VisualElement from the UIDocument
        var rootVisualElement = uiDocument.rootVisualElement;

        // Find the first Button element in the UI hierarchy
        var button = rootVisualElement.Q<Button>();
        if (button == null)
        {
            Debug.LogError("No Button found in the UI hierarchy.");
            return;
        }

        // Assign the Click event to load the scene with build index 1
        button.clicked += LoadScene;
    }

    private void LoadScene()
    {
        // Load the scene with build index 1
        SceneManager.LoadScene(1);
    }
}
