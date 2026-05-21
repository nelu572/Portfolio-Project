using PortfolioFilling.Core.Interfaces;
using UnityEngine;

namespace PortfolioFilling.Weapon
{
    public sealed class ProjectileDamage : MonoBehaviour
    {
        private float _damage;

        public void Initialize(float damage)
        {
            _damage = damage;
            Destroy(gameObject, 4f);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.TryGetComponent<IDamageable>(out var damageable))
            {
                damageable.TakeDamage(_damage);
            }

            Destroy(gameObject);
        }
    }
}
