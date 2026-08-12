using UnityEngine;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public sealed class XBALKeyboardAnimationPlayer : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private string armsLayerName = "Arms";
    [SerializeField] private string gunLayerName = "Gun";

    private static readonly int IdleHash = Animator.StringToHash("Idle");
    private static readonly int FireHash = Animator.StringToHash("Fire");
    private static readonly int ReloadHash = Animator.StringToHash("Reload");
    private static readonly int TacticalReloadHash = Animator.StringToHash("Tactical_Reload");
    private static readonly int InspectHash = Animator.StringToHash("Inspect");

    private int armsLayer;
    private int gunLayer;

    private void Reset()
    {
        animator = GetComponent<Animator>();
    }

    private void Awake()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        armsLayer = animator != null ? animator.GetLayerIndex(armsLayerName) : -1;
        gunLayer = animator != null ? animator.GetLayerIndex(gunLayerName) : -1;

        if (animator != null)
        {
            if (armsLayer >= 0)
            {
                animator.SetLayerWeight(armsLayer, 1f);
            }

            if (gunLayer >= 0)
            {
                animator.SetLayerWeight(gunLayer, 1f);
            }
        }
    }

    private void Update()
    {
        if (WasNumberPressed(1))
        {
            Play(IdleHash);
        }
        else if (WasNumberPressed(2))
        {
            Play(FireHash);
        }
        else if (WasNumberPressed(3))
        {
            Play(ReloadHash);
        }
        else if (WasNumberPressed(4))
        {
            Play(TacticalReloadHash);
        }
        else if (WasNumberPressed(5))
        {
            Play(InspectHash);
        }
    }

    private bool WasNumberPressed(int number)
    {
#if ENABLE_INPUT_SYSTEM
        var keyboard = Keyboard.current;
        if (keyboard == null)
        {
            return false;
        }

        return number switch
        {
            1 => keyboard.digit1Key.wasPressedThisFrame,
            2 => keyboard.digit2Key.wasPressedThisFrame,
            3 => keyboard.digit3Key.wasPressedThisFrame,
            4 => keyboard.digit4Key.wasPressedThisFrame,
            5 => keyboard.digit5Key.wasPressedThisFrame,
            _ => false
        };
#else
        return number switch
        {
            1 => Input.GetKeyDown(KeyCode.Alpha1),
            2 => Input.GetKeyDown(KeyCode.Alpha2),
            3 => Input.GetKeyDown(KeyCode.Alpha3),
            4 => Input.GetKeyDown(KeyCode.Alpha4),
            5 => Input.GetKeyDown(KeyCode.Alpha5),
            _ => false
        };
#endif
    }

    private void Play(int stateHash)
    {
        if (animator == null)
        {
            return;
        }

        if (armsLayer >= 0)
        {
            animator.Play(stateHash, armsLayer, 0f);
        }

        if (gunLayer >= 0)
        {
            animator.Play(stateHash, gunLayer, 0f);
        }

        animator.Update(0f);
    }
}
