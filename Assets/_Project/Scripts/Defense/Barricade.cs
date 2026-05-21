using UnityEngine;

namespace PortfolioFilling.Defense
{
    public sealed class Barricade : BuildableObject
    {
        [SerializeField] private float maxHealth = 100f;

        public float CurrentHealth { get; private set; }
        public float MaxHealth => maxHealth;

        private void Awake()
        {
            CurrentHealth = maxHealth;
        }

        public void TakeDamage(float damage)
        {
            CurrentHealth = Mathf.Max(0f, CurrentHealth - damage);
        }

        public void Repair(float amount)
        {
            CurrentHealth = Mathf.Min(maxHealth, CurrentHealth + amount);
        }
    }
}
