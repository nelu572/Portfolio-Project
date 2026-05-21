using PortfolioFilling.Core.Interfaces;
using UnityEngine;

namespace PortfolioFilling.Enemy
{
    public sealed class EnemyHealth : MonoBehaviour, IDamageable
    {
        private WaveManager _waveManager;
        private EnemyBase _owner;

        public float CurrentHealth { get; private set; }

        public void Initialize(float maxHealth, WaveManager waveManager, EnemyBase owner)
        {
            CurrentHealth = maxHealth;
            _waveManager = waveManager;
            _owner = owner;
        }

        public void TakeDamage(float amount)
        {
            CurrentHealth -= amount;
            if (CurrentHealth > 0f)
            {
                return;
            }

            _waveManager?.NotifyEnemyKilled(_owner);
            Destroy(gameObject);
        }
    }
}
