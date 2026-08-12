using UnityEngine;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public sealed class CreatorSeparatedKeyboardAnimationPlayer : MonoBehaviour
{
    [SerializeField] private Animator armsAnimator;
    [SerializeField] private Animator gunAnimator;

    private static readonly int IdleHash = Animator.StringToHash("idle_pose");
    private static readonly int FireHash = Animator.StringToHash("fire");
    private static readonly int ReloadHash = Animator.StringToHash("reload");
    private static readonly int WalkHash = Animator.StringToHash("walk");
    private static readonly int RefPoseHash = Animator.StringToHash("ref_pose");

    public void Configure(Animator arms, Animator gun)
    {
        armsAnimator = arms;
        gunAnimator = gun;
    }

    private void Update()
    {
        if (WasNumberPressed(1))
        {
            Play(IdleHash, IdleHash);
        }
        else if (WasNumberPressed(2))
        {
            Play(FireHash, FireHash);
        }
        else if (WasNumberPressed(3))
        {
            Play(ReloadHash, ReloadHash);
        }
        else if (WasNumberPressed(4))
        {
            Play(IdleHash, WalkHash);
        }
        else if (WasNumberPressed(5))
        {
            Play(RefPoseHash, RefPoseHash);
        }
    }

    private static bool WasNumberPressed(int number)
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

    private void Play(int armsStateHash, int gunStateHash)
    {
        PlayIfPresent(armsAnimator, armsStateHash);
        PlayIfPresent(gunAnimator, gunStateHash);
    }

    private static void PlayIfPresent(Animator animator, int stateHash)
    {
        if (animator == null || !animator.HasState(0, stateHash))
        {
            return;
        }

        animator.Play(stateHash, 0, 0f);
    }
}
