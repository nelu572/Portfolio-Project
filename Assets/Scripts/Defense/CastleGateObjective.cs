using UnityEngine;

public sealed class CastleGateObjective : MonoBehaviour, IDamageable
{
    [Header("성문")]
    [SerializeField] private string displayName = "성문";
    [SerializeField] private float maxHealth = 180f;

    private float currentHealth;
    private bool breached;

    public string DisplayName => displayName;

    public float CurrentHealth => currentHealth;

    public float MaxHealth => maxHealth;

    public bool IsAlive => currentHealth > 0f;

    public bool IsBreached => !IsAlive;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        if (!IsAlive)
        {
            return;
        }

        currentHealth = Mathf.Max(0f, currentHealth - amount);
        if (!IsAlive && !breached)
        {
            breached = true;
            OpenGate();
        }
    }

    private void OpenGate()
    {
        foreach (var gateCollider in GetComponentsInChildren<Collider>())
        {
            gateCollider.enabled = false;
        }

        transform.localPosition += Vector3.down * 2.4f;
    }
}
