using UnityEngine;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public sealed class CreatorReloadKeyboardAnimationPlayer : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private static readonly int IdleHash = Animator.StringToHash("Idle");
    private static readonly int FireHash = Animator.StringToHash("Fire");
    private static readonly int ReloadHash = Animator.StringToHash("Reload");
    private static readonly int WalkHash = Animator.StringToHash("Walk");
    private static readonly int RefPoseHash = Animator.StringToHash("RefPose");

    public void Configure(Animator targetAnimator)
    {

    }

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
            Play(WalkHash);
        }
        else if (WasNumberPressed(5))
        {
            Play(RefPoseHash);
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

    private void Play(int stateHash)
    {
        if (animator == null || !animator.HasState(0, stateHash))
        {
            return;
        }

        animator.Play(stateHash, 0, 0f);
    }
}
