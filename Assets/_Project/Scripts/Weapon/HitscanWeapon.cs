using PortfolioFilling.Core.Interfaces;
using UnityEngine;

namespace PortfolioFilling.Weapon
{
    public sealed class HitscanWeapon : WeaponBase
    {
        protected override void FireShot(Vector3 origin, Vector3 direction)
        {
            if (!Physics.Raycast(origin, direction, out var hit, Data.range))
            {
                return;
            }

            if (hit.collider.TryGetComponent<IDamageable>(out var damageable))
            {
                damageable.TakeDamage(Data.damage);
            }
        }
    }
}
