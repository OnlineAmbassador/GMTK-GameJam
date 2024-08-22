using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontTurretCamera : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float turnSpeed = 10f;
    public float turretTurnSpeed = 50f;
    public float camsens = 10;
    public float fireRate = .5f;
    public float fireTime = .5f;
    public GameObject bullet;
    public Transform turret; // The turret's transform
    public Transform cameraTransform; // The camera's transform
    public Vector3 localStart;
    public Vector3 localStartB;
    public Transform anothaOne;
    public float scale;

    // Clamping angles for camera vertical rotation
    public float minCameraAngle = -60f;
    public float maxCameraAngle = 60f;

    private CharacterController characterController;
    private float currentCameraRotationX = 0f; // To track current vertical rotation

    void Start()
    {
        localStart = anothaOne.localPosition;
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        localStartB = cameraTransform.localPosition;
    }

    void Update()
    {
        float turretHorizontal = Input.GetAxis("Mouse X") * camsens;
        float turretVertical = Input.GetAxis("Mouse Y") * camsens;

        // Rotate the turret horizontally
        turret.Rotate(0f, turretHorizontal * turretTurnSpeed * Time.deltaTime, 0);

        // Calculate the new vertical camera rotation
        currentCameraRotationX -= turretVertical * turretTurnSpeed * Time.deltaTime;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, minCameraAngle, maxCameraAngle);

        // Apply the clamped vertical rotation to the camera
        cameraTransform.localRotation = Quaternion.Euler(currentCameraRotationX, 0f, 0f);

        if (!PlayerMovement.isBigMode && Time.timeScale != 0 && !Input.GetMouseButtonUp(0))
        {
            if (Input.GetMouseButton(0))
            {
                fireTime -= Time.deltaTime;
                if (fireTime < 0)
                {
                    anothaOne.localPosition = new Vector3(localStart.x + (Random.value - .5f) * scale, localStart.y + (Random.value - .5f) * scale, localStart.z + (Random.value - .5f) * scale);
                    cameraTransform.localPosition = new Vector3(localStartB.x + (Random.value - .5f) * scale, localStartB.y + (Random.value - .5f) * scale, localStartB.z + (Random.value - .5f) * scale);
                    fireTime = fireRate;
                    Instantiate(bullet, cameraTransform.position + cameraTransform.forward * 6 - cameraTransform.up, cameraTransform.rotation);
                }
            }
        
            if (Input.GetMouseButtonDown(0))
            {
                GameObject.Find("TrainController").GetComponentInChildren<AudioSource>().Play();
            }
        }
        else
        {
              anothaOne.localPosition = new Vector3(localStart.x, localStart.y, localStart.z);
              GameObject.Find("TrainController").GetComponentInChildren<AudioSource>().Stop();
        }
    }
}

