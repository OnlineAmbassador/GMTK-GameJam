using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class HideControlsOnKeyPress : MonoBehaviour
{
    [SerializeField] KeyCode targetKey = KeyCode.UpArrow; // Ze key to detect
    [SerializeField] float holdDuration = 3f; // Seconds btw
    [SerializeField] float moveDuration = 1f;
    [SerializeField] float moveDistance = 1000f;
    [SerializeField] AnimationCurve easeCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private float holdStartTime;
    private bool isHolding = false;
    private bool hasMoved = false;

    ProgressBar progressBar;

    private VisualElement controlsPanel;
    private UIDocument uiDocument;

    private void Start()
    {
        uiDocument = GetComponent<UIDocument>();
        uiDocument.enabled = true;
        if (uiDocument == null)
        {
            Debug.LogError("UIDocument component not found!");
            return;
        }

        controlsPanel = uiDocument.rootVisualElement.Q("ControlsPanel");
        if (controlsPanel == null)
        {
            Debug.LogError("ControlsPanel not found in the UI Document!");
        }

        progressBar = uiDocument.rootVisualElement.Q<ProgressBar>("pb");
    }

    void Update()
    {
        if (Input.GetKeyDown(targetKey))
        {
            holdStartTime = Time.time;
            uiDocument.rootVisualElement.Q("AccKey").AddToClassList("acckeyactive");
            isHolding = true;
        }

        if (Input.GetKeyUp(targetKey))
        {
            isHolding = false;
            uiDocument.rootVisualElement.Q("AccKey").RemoveFromClassList("acckeyactive");
        }

        if (isHolding && !hasMoved && Time.time - holdStartTime >= holdDuration)
        {
            MovePanel();
            hasMoved = true;
        }

        if (isHolding)
        {
            progressBar.value = (Time.time - holdStartTime)/holdDuration * 100;
        }
        else if (Time.time - holdStartTime <= holdDuration)
        {
            progressBar.value = 0;
        }
    }


    void MovePanel()
    {
        if (controlsPanel != null)
        {
            StartCoroutine(SmoothMove());
        }
    }

    IEnumerator SmoothMove()
    {
        float elapsedTime = 0;
        Vector3 startPosition = controlsPanel.transform.position;
        Vector3 endPosition = startPosition - new Vector3(moveDistance, 0, 0);

        while (elapsedTime < moveDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / moveDuration);
            float easedT = easeCurve.Evaluate(t);
            controlsPanel.transform.position = Vector3.Lerp(startPosition, endPosition, easedT);
            yield return null;
        }

        controlsPanel.transform.position = endPosition;
    }
}