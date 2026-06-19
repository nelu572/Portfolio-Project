using UnityEngine;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

[RequireComponent(typeof(CharacterController))]
public sealed class GrapplingHookPrototype : MonoBehaviour
{
    [Header("그래플링 훅")]
    [SerializeField] private Transform[] grapplePoints;
    [SerializeField] private float range = 28f;
    [SerializeField] private float travelSpeed = 18f;
    [SerializeField] private float cooldown = 2.2f;

    private CharacterController characterController;
    private Transform activePoint;
    private float cooldownTimer;

    public float Cooldown01 => cooldown <= 0f ? 0f : Mathf.Clamp01(cooldownTimer / cooldown);

    public bool IsGrappling => activePoint != null;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        cooldownTimer -= Time.deltaTime;

        if (activePoint != null)
        {
            MoveToActivePoint();
            return;
        }

        if (WasGrapplePressed() && cooldownTimer <= 0f)
        {
            activePoint = FindBestPoint();
            if (activePoint != null)
            {
                cooldownTimer = cooldown;
            }
        }
    }

    public void SetGrapplePoints(Transform[] points)
    {
        grapplePoints = points;
    }

    private void MoveToActivePoint()
    {
        var target = activePoint.position;
        var toTarget = target - transform.position;

        if (toTarget.magnitude <= 0.85f)
        {
            activePoint = null;
            return;
        }

        characterController.Move(toTarget.normalized * travelSpeed * Time.deltaTime);
    }

    private Transform FindBestPoint()
    {
        Transform bestPoint = null;
        var bestScore = float.MaxValue;

        foreach (var point in grapplePoints)
        {
            if (point == null)
            {
                continue;
            }

            var distance = Vector3.Distance(transform.position, point.position);
            if (distance > range || distance >= bestScore)
            {
                continue;
            }

            bestScore = distance;
            bestPoint = point;
        }

        return bestPoint;
    }

    private static bool WasGrapplePressed()
    {
#if ENABLE_INPUT_SYSTEM
        return Mouse.current != null && Mouse.current.rightButton.wasPressedThisFrame;
#else
        return Input.GetMouseButtonDown(1);
#endif
    }
}
