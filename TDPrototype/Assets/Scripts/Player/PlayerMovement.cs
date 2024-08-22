using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class PlayerMovement : MonoBehaviour
{
    // Public variables
    public static bool isBigMode = false;
    float xRot;
    float yRot;
    float sensitivity;
    Vector2 playerMouseInput;
    Vector3 playerMovementInput;
    Vector3 velocity;
    [SerializeField] Transform playerCamera;
    [SerializeField] CharacterController controller;
    [Space]
    [SerializeField] float speed;
    [SerializeField] float jumpForce;
    [SerializeField] float gravity = -9.86f;

    public Texture2D cursor;
    public TextMeshProUGUI textMeshProUGUI;
    public Camera cam;

    // Clamping angles for camera vertical rotation
    public float minCameraAngle = -60f;
    public float maxCameraAngle = 60f;

    // Variables for holding E to switch perspective
    float eKeyHoldTime = 0f;
    const float holdDuration = 1f; // Duration to hold the E key

    void Start()
    {
        Cursor.SetCursor(cursor, Vector3.zero, CursorMode.ForceSoftware);
    }

    void Update()
    {
        sensitivity = SettingsHandler.Sensitivity;

        if (isBigMode)
        {
            MovePlayer();
            playerMovementInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
            playerMouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            MoveCamera();
            PressEToBuy();
        }
        else
        {
            textMeshProUGUI.text = "";
        }
    }

    private void PressEToBuy()
    {
        textMeshProUGUI.text = "";
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        var hitbool = Physics.Raycast(ray, out hit, 100f);
        if (hitbool)
        {
            if (hit.collider.CompareTag("trainpickup1"))
            {
                int yourcash = EnemySpawnerScript.totalMoney;
                if (yourcash >= 10)
                {
                    textMeshProUGUI.text = "Press E to buy Turret Cart for 10 bucks. You have " + yourcash.ToString();
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        GameObject.FindFirstObjectByType<TrainGameController>().AddCart();
                        EnemySpawnerScript.totalMoney -= 10;
                    }
                }
                else
                {
                    textMeshProUGUI.text = "Can't buy, it costs 10. You have " + yourcash.ToString();
                }
            } else if (hit.collider.CompareTag("trainpickup2"))
            {
                int yourcash = EnemySpawnerScript.totalMoney;
                if (yourcash >= 20)
                {
                    textMeshProUGUI.text = "Press E to buy Tesla Cart for 20 bucks. You have " + yourcash.ToString();
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        GameObject.FindFirstObjectByType<TrainGameController>().AddCart(1);
                        EnemySpawnerScript.totalMoney -= 20;
                    }
                }
                else
                {
                    textMeshProUGUI.text = "Can't buy, it costs 20. You have " + yourcash.ToString();
                }
            }
            else
            {
                // Nothing else is hovered
                HandlePerspectiveSwitch();
            }
        }
        else
        {
            // Nothing else is hovered
            HandlePerspectiveSwitch();
        }
    }

    private void MovePlayer()
    {
        if (controller.isGrounded)
        {
            velocity.y = -1f;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                velocity.y = jumpForce;
            }
        }
        else
        {
            velocity.y -= gravity * -2f * Time.deltaTime;
        }

        Vector3 moveVector = transform.TransformDirection(playerMovementInput);
        controller.Move(moveVector * speed * Time.deltaTime);
        controller.Move(velocity * Time.deltaTime);
    }

    private void MoveCamera()
    {
        xRot -= playerMouseInput.y * sensitivity * Time.deltaTime;
        yRot += playerMouseInput.x * sensitivity * Time.deltaTime;

        // Clamp the vertical rotation
        xRot = Mathf.Clamp(xRot, minCameraAngle, maxCameraAngle);

        // Apply rotation
        transform.localRotation = Quaternion.Euler(0f, yRot, 0f);
        playerCamera.transform.localRotation = Quaternion.Euler(xRot, 0f, 0f);
    }

    private void HandlePerspectiveSwitch()
    {
        if (Input.GetKey(KeyCode.E))
        {
            eKeyHoldTime += Time.deltaTime;

            startButtonController.buttonlight.intensity = eKeyHoldTime * 10f;

            if (eKeyHoldTime >= holdDuration)
            {
                GameObject.FindFirstObjectByType<EnemySpawnerScript>().nextWave();
                SwitchPerspective();
                eKeyHoldTime = 0f; // Reset the hold time after switching perspective
            }
        }
        else
        {
            startButtonController.buttonlight.intensity = 0f;
            eKeyHoldTime = 0f; // Reset hold time if the key is released
        }
    }

    public static void SwitchPerspective()
    {
        isBigMode = !isBigMode;
        GameObject.Find("FrontCart").GetComponentInChildren<Camera>().enabled = !isBigMode;
        if (isBigMode)
        {
            Debug.Log("SWITCHING FROM SMALL to BIG");
            Cursor.lockState = CursorLockMode.Locked;
            GameObject.Find("FrontCart").GetComponentInChildren<AudioSource>().Pause();
            GameObject.Find("BigGuyCamera").GetComponentInChildren<AudioSource>().Play();
        }
        else
        {
            Debug.Log("SWITCHING FROM BIG to SMALL");
            GameObject.Find("FrontCart").GetComponentInChildren<AudioSource>().Play();
            GameObject.Find("BigGuyCamera").GetComponentInChildren<AudioSource>().Pause();
        }
        GameObject.Find("BigGuyCamera").GetComponentInChildren<Camera>().enabled = isBigMode;
    }
}
