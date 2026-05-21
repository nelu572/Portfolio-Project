using PortfolioFilling.Core;
using PortfolioFilling.Defense;
using UnityEngine;

namespace PortfolioFilling.Enemy
{
    public sealed class EnemyBase : MonoBehaviour
    {
        private ZombieAI _ai;
        private EnemyHealth _health;

        public void Initialize(Transform playerTarget, DefenseObjective objective, ConfigManager configManager, WaveManager waveManager)
        {
            _ai = gameObject.AddComponent<ZombieAI>();
            _health = gameObject.AddComponent<EnemyHealth>();
            var attack = gameObject.AddComponent<EnemyAttack>();

            _health.Initialize(configManager.Runtime.zombieMaxHealth, waveManager, this);
            _ai.Initialize(playerTarget, objective.transform, configManager.Runtime.zombieMoveSpeed);
            attack.Initialize(_ai, configManager.Runtime.zombieAttackDamage, configManager.Runtime.zombieAttackRange, configManager.Runtime.zombieAttackCooldown);
        }

        public Transform CurrentTarget => _ai == null ? null : _ai.CurrentTarget;
    }
}
