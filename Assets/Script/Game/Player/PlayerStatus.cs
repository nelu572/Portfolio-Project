using DG.Tweening;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public static PlayerStatus Instance;

    void Awake()
    {
        if (Instance != null)// 중복 방지
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    [SerializeField] private float dashCoolDown = 5f;
    private bool canDash = true;
    private Tween dashCooldownTween;

    public bool CanDash => canDash;
    public float MAXdashCoolDown => dashCoolDown;

    public void UseDash()
    {
        if (!canDash) return;

        canDash = false;

        dashCooldownTween = DOVirtual.DelayedCall(dashCoolDown, () =>
        {
            canDash = true;
        });
    }

    public float GetDashRemainingNormalized()
    {
        if (dashCooldownTween == null || !dashCooldownTween.IsActive())
            return 0f;

        return 1f - (dashCooldownTween.Elapsed() / dashCooldownTween.Duration());
    }
}
