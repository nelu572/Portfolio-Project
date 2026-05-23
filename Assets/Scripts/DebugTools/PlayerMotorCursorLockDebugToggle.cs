using UnityEngine;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public sealed class PlayerMotorCursorLockDebugToggle : MonoBehaviour
{
    [SerializeField] private PlayerMotor playerMotor;

#if !ENABLE_INPUT_SYSTEM
    [SerializeField] private KeyCode toggleKey = KeyCode.L;
#endif

    private void Reset()
    {
        playerMotor = GetComponent<PlayerMotor>();
    }

    private void Update()
    {
        if (playerMotor == null || !WasToggleKeyPressed())
        {
            return;
        }

        playerMotor.LockCursorOnPlay = !playerMotor.LockCursorOnPlay;
    }

    private bool WasToggleKeyPressed()
    {
#if ENABLE_INPUT_SYSTEM
        return Keyboard.current != null && Keyboard.current.lKey.wasPressedThisFrame;
#else
        return Input.GetKeyDown(toggleKey);
#endif
    }
}
