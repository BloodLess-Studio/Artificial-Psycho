using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMouvement : MonoBehaviour
{
    /*------ VARIABLES ------*/
    //Parameters
    [Header("Player Settings")]
    public float playerHeight = 2f;

    [Header("Groud Detection")]
    [SerializeField] LayerMask groundMask;
    [SerializeField] Transform groundCheck;
    public float groundDetectionRadius = 0.4f;
    public bool isGrounded;

    [Header("Basics Movement")]
    public float moveSpeed = 5f;
    public float airSpeedRatio = 0.1f;
    public float playerDrag = 6f;
    public float airDrag = 0.5f;

    [Header("Sprinting")]
    [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float acceleration = 10f;

    [Header("Jump")]
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    public float jumpForce = 8f;

    //Private Variables
    private Rigidbody rb;
    private RaycastHit slopeHit;

    private float horizontalMovement;
    private float verticalMovement;
    private Vector3 moveDirection;
    private Vector3 slopeMoveDirection;

    /*------ METHODS ------*/

    // Start()
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    // Update()
    #region Update 
    private void Update()
    {
        CheckGrounded();    //Check is the player is grounded or not
        ReadInput();        //Get input and set movement vector
        HandleDrag();       //Set player drag => (Le Drag est grossomodo la meme chose que la résistance de l'air. On est met ici pour adoucir le mouvement)
        CheckRunning();
        CheckJump();        //Check is the player WANT and CAN jump => if yes, make the player jump.
        CheckSlope();      
    }

    private void CheckGrounded()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDetectionRadius, groundMask);
    }

    private void ReadInput()
    {
        //Get the inputs
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");

        moveDirection = transform.forward * verticalMovement + transform.right * horizontalMovement; // Calculate the movement vector
    }

    private void HandleDrag()
    {
        if (isGrounded) rb.drag = playerDrag;
        else rb.drag = airDrag;                 //Set the drag in function of if the player is grounded or not
    }

    private void CheckSlope()
    {
        slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);    
    }

    private void CheckRunning()
    {
        if (Input.GetKey(sprintKey) && isGrounded)
            moveSpeed = Mathf.Lerp(moveSpeed, runSpeed, acceleration * Time.deltaTime);
        else
            moveSpeed = Mathf.Lerp(moveSpeed, walkSpeed, acceleration * Time.deltaTime);

    }

    private void CheckJump()
    {
        if (Input.GetKeyDown(jumpKey) && isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z); //Reset the UP/DOWN velocity of the player => Fix the bug where the jump would be shorter cause the player hadn't touch the groud yet.
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
            
    }

    private bool OnSlope()
        {
            if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.5f))
            {
                if (slopeHit.normal != Vector3.up) return true;
                return false;
            }
            return false;
        }

    #endregion

    // FixedUpdate()
    #region FixedUpdate
    private void FixedUpdate()
    {
        HandleGravity();    //Disable gravity when the player is grouded => Optimization + bugfix sliding off slope
        MovePlayer();   //Move the player according to the movement vector        
    }

    private void HandleGravity()
    {
        if (isGrounded)
            rb.useGravity = false;
        else if (!isGrounded)
            rb.useGravity = true;
    }

    private void MovePlayer()
    {
        if (isGrounded && !OnSlope())
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Acceleration); //Add accesleration to the player with the movement vector
        else if (isGrounded && OnSlope())
            rb.AddForce(slopeMoveDirection.normalized * moveSpeed * 10f, ForceMode.Acceleration);   //Same but we use slope mmovement vector instead
        else if (!isGrounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * airSpeedRatio * 10f, ForceMode.Acceleration);    //Same but with the air speed ratio
    }
    #endregion
}
