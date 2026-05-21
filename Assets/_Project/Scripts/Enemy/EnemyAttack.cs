using PortfolioFilling.Core.Interfaces;
using UnityEngine;

namespace PortfolioFilling.Enemy
{
    public sealed class EnemyAttack : MonoBehaviour
    {
        private ZombieAI _ai;
        private float _damage;
        private float _range;
        private float _cooldown;
        private float _nextAttackAt;

        public void Initialize(ZombieAI ai, float damage, float range, float cooldown)
        {
            _ai = ai;
            _damage = damage;
            _range = range;
            _cooldown = cooldown;
        }

        private void Update()
        {
            if (_ai == null || _ai.CurrentTarget == null || Time.time < _nextAttackAt)
            {
                return;
            }

            if (Vector3.Distance(transform.position, _ai.CurrentTarget.position) > _range)
            {
                return;
            }

            if (_ai.CurrentTarget.TryGetComponent<IDamageable>(out var damageable))
            {
                damageable.TakeDamage(_damage);
                _nextAttackAt = Time.time + _cooldown;
            }
        }
    }
}
