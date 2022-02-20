using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Handle player movement and physics.
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : InterpolatedTransform
{
    /*-------- Unity Inspector --------*/

    [Header("Movement Parameters")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 10f;
    public float crouchSpeed = 3f;

    [Header("Jump Parameters")]
    public float jumpSpeed = 8f;
    public Vector3 jumpVector = Vector3.zero;

    [Header("Physics")]
    [SerializeField] private float gravity = 20f;
    [SerializeField] private float antiBumpFactor = 0.75f;

    [Header("Groud Detection")]
    [SerializeField] Transform groundCheck;
    public float groundDetectionRadius = 0.4f;
    public bool isGrounded;

    /*-------- Variables --------*/

    //Class Requirement
    UnityEvent onReset = new UnityEvent();
    private float offTime = 0f; // Time during which the player cant move (e.g when a movement is already playing)
    [HideInInspector] public CharacterController controller;
    [HideInInspector] public bool playerControl = false;

    //Physics
    [HideInInspector] public Vector3 moveDirection = Vector3.zero;
    [HideInInspector] public Vector3 contactPoint;
    private bool GravityEnable;

    //Jump
    private Vector3 jumpDirection;
    private float jumpPower;

    /*-------- Unity Starting Event --------*/
    #region 
    public override void OnEnable()
    {
        base.OnEnable();
        controller = GetComponent<CharacterController>();
    }

    public void Awake()
    {
        controller = GetComponent<CharacterController>();
    }
    #endregion
    /*-------- OnReset Event --------*/
    #region
    /// <summary>
    /// Add an UnityAction to the event 'onReset'
    /// </summary>
    /// <param name="call">(UnityAction)Action to perfom when onReset is triggered</param>
    public void AddToReset(UnityAction call)
    {
        onReset.AddListener(call);
    }
    #endregion
    /*-------- Unity Update Event --------*/
    #region
    public override void Update()
    {
        // As we are not using the base, we need to manually get the last position and the oldest
        Vector3 newestTransform = m_lastPositions[m_newTransformIndex];
        Vector3 olderTransform = m_lastPositions[OldTransformIndex()];

        // We lerp the two position (We get aproximatly the middle between the two)
        Vector3 adjust = Vector3.Lerp(olderTransform, newestTransform, InterpolationController.InterpolationFactor);
        adjust -= transform.position;

        // We move the player according to the interpollation
        controller.Move(adjust);

        // Decrease the offTime
        if (offTime > 0) offTime -= Time.deltaTime;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        // If there is still offTime, continue the current movement and update 'isGrounded'
        if (offTime > 0)
        {
            // Apply gravity
            if (GravityEnable) moveDirection.y -= gravity * Time.deltaTime;
            // Move and check if grounded
            controller.Move(moveDirection * Time.deltaTime);
            CheckGrounded();
        }
    }
    #endregion
    /*-------- Public Methods --------*/
    #region
    /// <summary>
    /// Move the CharacterController
    /// </summary>
    /// <param name="input">(Vector2)Movement input</param>
    /// <param name="sprint">(Bool)Is the player sprinting?</param>
    /// <param name="crouching">(Bool)Is the player crouching?</param>
    public void Move(Vector2 input, bool sprint, bool crouching)
    {
        // If there is offTime left, refuse the movement
        if (offTime > 0) return;

        // Set the movement speed according to the movement type
        float speed = (!sprint) ? walkSpeed : sprintSpeed;
        if (crouching) speed = crouchSpeed;

        if (isGrounded)
        {
            // Get the movement direction from the input
            moveDirection = new Vector3(input.x, -antiBumpFactor, input.y);
            moveDirection = transform.TransformDirection(moveDirection) * speed;

            // Handle jump
            UpdateJump();
        }
        else
        {
            /*
             This modify the jump trajectory according to the input
             */

            // Get a direction from input
            Vector3 adjust = new Vector3(input.x, 0, input.y);
            adjust = transform.TransformDirection(adjust);
            // Add that new direction to the jump direction
            jumpDirection += adjust * Time.fixedDeltaTime * jumpPower * 2f;
            jumpDirection = Vector3.ClampMagnitude(jumpDirection, jumpPower);
            // Set the move direction
            moveDirection.x = jumpDirection.x;
            moveDirection.z = jumpDirection.z;
        }

        // Apply gravity
        moveDirection.y -= gravity * Time.deltaTime;
        
        // Move and check if grounded
        controller.Move(moveDirection * Time.deltaTime);
        CheckGrounded();
    }

    /// <summary>
    /// Move the CharacterController
    /// </summary>
    /// <param name="direction">(Vector3)Movement direction</param>
    /// <param name="speed">(Float)movement speed</param>
    /// <param name="appliedGravity">(Float)Applied gravity</param>
    public void Move(Vector3 direction, float speed, float appliedGravity)
    {
        // If there is offTime left, refuse the movement
        if (offTime > 0) return;

        // Set moveDirection according to 'appliedGravity'
        Vector3 move = direction * speed;
        if (appliedGravity > 0)
        {
            moveDirection.x = move.x;
            moveDirection.y -= gravity * Time.deltaTime * appliedGravity;
            moveDirection.z = move.z;
        }
        else
            moveDirection = move;

        // Handle jump
        UpdateJump();

        // Move and check if grounded
        controller.Move(moveDirection * Time.deltaTime);
        CheckGrounded();
    }

    /// <summary>
    /// Move the CharacterController
    /// </summary>
    /// <param name="direction">(Vector3)Movement direction</param>
    /// <param name="speed">(Float)movement speed</param>
    /// <param name="appliedGravity">(Float)Applied gravity</param>
    /// <param name="setY">(Float)Y value to be set</param>
    public void Move(Vector3 direction, float speed, float appliedGravity, float setY)
    {
        // If there is offTime left, refuse the movement
        if (offTime > 0) return;

        // Set moveDirection according to 'appliedGravity' and 'setY'
        Vector3 move = direction * speed;
        if (appliedGravity > 0)
        {
            moveDirection.x = move.x;
            if (setY != 0) moveDirection.y = setY * speed;
            moveDirection.y -= gravity * Time.deltaTime * appliedGravity;
            moveDirection.z = move.z;
        }
        else
            moveDirection = move;

        // Handle jump
        UpdateJump();

        // Move and check if grounded
        controller.Move(moveDirection * Time.deltaTime);
        CheckGrounded();
    }

    /// <summary>
    /// Force the player to do a movement, ignoring offtime, current gravity and any other constraint.
    /// </summary>
    /// <param name="direction">(Vector3)Movement direction</param>
    /// <param name="speed">(Float)Movement speed</param>
    /// <param name="time">(Float)OffTime needed for that movement</param>
    /// <param name="applyGravity">(Bool)Is gravity enable?</param>
    public void ForceMove(Vector3 direction, float speed, float time, bool applyGravity)
    {
        offTime = time;
        GravityEnable = applyGravity;
        moveDirection = direction * speed;
    }

    /// <summary>
    /// Update JumpVector.
    /// </summary>
    /// <param name="dir">(Vector3)Movement direction</param>
    /// <param name="mult">(Float)Jump multiplier</param>
    public void Jump(Vector3 dir, float mult)
    {
        jumpVector = dir * mult;
    }
    #endregion
    /*-------- Private Methods --------*/
    #region
    /// <summary>
    /// Update 'isGrounded'. Use a sphere collider.
    /// </summary>
    private void CheckGrounded()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDetectionRadius);
    }

    /// <summary>
    /// Update movment direction and jump power according to current jump vector
    /// </summary>
    private void UpdateJump()
    {
        // If there is a jump movement
        if (jumpVector != Vector3.zero)
        {
            // Set moveDirection with jumpVector value
            Vector3 dir = (jumpVector * jumpSpeed);
            if (dir.x != 0) moveDirection.x = dir.x;
            if (dir.y != 0) moveDirection.y = dir.y;
            if (dir.z != 0) moveDirection.z = dir.z;

            // Set the jump power according to movement direction
            Vector3 move = moveDirection;
            jumpDirection = move; move.y = 0;
            jumpPower = Mathf.Min(move.magnitude, jumpSpeed); // This needs to be commented
            jumpPower = Mathf.Max(jumpPower, walkSpeed); // Also this
        }

        // Reset jumpVector
        jumpVector = Vector3.zero;
    }
    #endregion
}
