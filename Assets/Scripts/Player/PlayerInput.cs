using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(UnityEngine.InputSystem.PlayerInput))]
public sealed class PlayerInput : MonoBehaviour
{
    private Vector2 move;
    private Vector2 look;
    private bool jumpPressedThisFrame;
    private bool sprintHeld;

    public Vector2 Move => move;

    public Vector2 Look => look;

    public bool JumpPressedThisFrame => jumpPressedThisFrame;

    public bool SprintHeld => sprintHeld;

    private void OnDisable()
    {
        move = Vector2.zero;
        look = Vector2.zero;
        jumpPressedThisFrame = false;
        sprintHeld = false;
    }

    private void LateUpdate()
    {
        look = Vector2.zero;
        jumpPressedThisFrame = false;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            move = context.ReadValue<Vector2>();
        }
        else if (context.canceled)
        {
            move = Vector2.zero;
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            look = context.ReadValue<Vector2>();
        }
        else if (context.canceled)
        {
            look = Vector2.zero;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            jumpPressedThisFrame = true;
        }
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            sprintHeld = context.ReadValueAsButton();
        }
        else if (context.canceled)
        {
            sprintHeld = false;
        }
    }
}
