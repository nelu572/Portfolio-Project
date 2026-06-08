using UnityEngine;

public sealed class PlayerHealth : MonoBehaviour, IDamageable
{
    [Header("플레이어 체력")]
    [SerializeField] private float maxHealth = 100f;

    private float currentHealth;

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
