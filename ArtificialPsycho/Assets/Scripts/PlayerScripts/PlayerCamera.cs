using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    /*------ VARIABLES ------*/
    //Parameters
    [Header("Mouse Sensitivity")]
    [SerializeField] private float sensitivityVertical = 20f;
    [SerializeField] private float sensitivityHorizontal = 20f;

    [Header("Camera Settings")]
    public float clampingAngle = 90f;

    //Private Variables
    private Camera playerCamera;
    private Rigidbody rb;

    private float mouseX;
    private float mouseY;

    private float xRotation;
    private float yRotation;

    /*------ METHODS ------*/
    // Start()
    private void Start()
    {
        playerCamera = GetComponentInChildren<Camera>();
        rb = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update()
    private void Update()
    {
        ReadInput();
    }

    private void ReadInput()
    {
        mouseX = Input.GetAxisRaw("Mouse X");
        mouseY = Input.GetAxisRaw("Mouse Y");

        yRotation += mouseX * sensitivityHorizontal * 0.1f;
        xRotation -= mouseY * sensitivityVertical * 0.1f;

        xRotation = Mathf.Clamp(xRotation, -clampingAngle, clampingAngle);
    }

    // FixedUpdate
    private void FixedUpdate()
    {
        RotateCamera();
        RotatePlayerBody();
    }

    private void RotateCamera()
    {
        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
    }

    private void RotatePlayerBody()
    {
        rb.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
