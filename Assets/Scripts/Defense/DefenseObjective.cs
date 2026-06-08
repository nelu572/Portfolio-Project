using UnityEngine;

public sealed class DefenseObjective : MonoBehaviour, IDamageable
{
    [Header("방어 목표물")]
    [SerializeField] private string displayName = "왕";
    [SerializeField] private float maxHealth = 250f;

    private float currentHealth;

    public string DisplayName => displayName;

    public float CurrentHealth => currentHealth;

    public float MaxHealth => maxHealth;

    public bool IsAlive => currentHealth > 0f;

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
    }
}
