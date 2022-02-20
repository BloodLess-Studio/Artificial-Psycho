using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    /*-------- Private Variables --------*/

    private Vector2 _input;
    private Vector2 _look;
    private bool _sprint;
    private bool _jump;
    private bool _crouch;

    /*-------- Getters/Setters --------*/

    public Vector2 MovementInput
    {
        get
        {
            //This line is used to smooth out the movement
            _input *= (_input.x != 0f && _input.y != 0f) ? 0.7071f : 1f;
            return _input;
        }
    }

    public Vector2 MouseInput => _look;
    public bool Sprint => _sprint;
    public bool Jump => _jump;
    public bool Crouch => _crouch;

    /*-------- Inputs Events --------*/

    /// <summary>
    /// Set input vector.
    /// Is called when Movement Input is triggered
    /// </summary>
    /// <param name="value">(Vector2)Movement value</param>
    public void OnMovement(InputValue value)
    {
        _input = value.Get<Vector2>();
    }

    /// <summary>
    /// Set mouse vector.
    /// Is called when Look Input is triggered
    /// </summary>
    /// <param name="value">(Vector2)Mouse value</param>
    public void OnLook(InputValue value)
    {
        _look = value.Get<Vector2>();
    }

    /// <summary>
    /// Set jump variable.
    /// Is called when Jump Input is triggered
    /// </summary>
    /// <param name="value">(Bool)Jump value</param>
    public void OnJump(InputValue value)
    {
        _jump = value.isPressed;
    }

    /// <summary>
    /// Set sprint variable.
    /// Is called when Sprint Input is triggered
    /// </summary>
    /// <param name="value">(Bool)Sprint value</param>
    public void OnSprint(InputValue value)
    {
        _sprint = value.isPressed;
    }

    /// <summary>
    /// Set crouch variable.
    /// Is called when Crouch Input is triggered
    /// </summary>
    /// <param name="value">(Bool)Crouch value</param>
    public void OnCrouch(InputValue value)
    {
        _crouch = value.isPressed;
    }
}
