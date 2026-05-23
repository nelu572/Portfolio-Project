using UnityEngine;

public sealed class PlayerMotor : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Transform playerRoot;
    [SerializeField] private Transform cameraPivot;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float sprintMultiplier = 1.6f;
    [SerializeField] private float jumpHeight = 1.2f;
    [SerializeField] private float gravity = -25f;

    [Header("Look")]
    [SerializeField] private float mouseSensitivity = 2.5f;
    [SerializeField] private float maxLookAngle = 85f;
    [SerializeField] private bool lockCursorOnPlay = true;

    private float verticalVelocity;
    private float pitch;

    private void Start()
    {
        if (lockCursorOnPlay)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void Update()
    {
        if (playerInput == null || characterController == null || playerRoot == null || cameraPivot == null)
        {
            return;
        }

        UpdateLook();
        UpdateMovement();
    }

    private void UpdateLook()
    {
        var lookDelta = playerInput.Look * mouseSensitivity * 0.1f;
        pitch = Mathf.Clamp(pitch - lookDelta.y, -maxLookAngle, maxLookAngle);

        playerRoot.Rotate(Vector3.up * lookDelta.x);
        cameraPivot.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }

    private void UpdateMovement()
    {
        var moveInput = playerInput.Move;
        var moveDirection = (playerRoot.forward * moveInput.y) + (playerRoot.right * moveInput.x);
        var currentSpeed = moveSpeed * (playerInput.SprintHeld ? sprintMultiplier : 1f);

        if (characterController.isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = -2f;
        }

        if (characterController.isGrounded && playerInput.JumpPressedThisFrame)
        {
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        verticalVelocity += gravity * Time.deltaTime;

        var velocity = moveDirection * currentSpeed;
        velocity.y = verticalVelocity;

        characterController.Move(velocity * Time.deltaTime);
    }
}
