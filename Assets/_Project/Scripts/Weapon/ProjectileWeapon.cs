using UnityEngine;

namespace PortfolioFilling.Weapon
{
    public sealed class ProjectileWeapon : WeaponBase
    {
        protected override void FireShot(Vector3 origin, Vector3 direction)
        {
            var projectile = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            projectile.name = "HarnessProjectile";
            projectile.transform.position = origin + direction * 0.4f;
            projectile.transform.localScale = Vector3.one * 0.2f;

            var rigidbody = projectile.AddComponent<Rigidbody>();
            rigidbody.useGravity = false;
            rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            rigidbody.linearVelocity = direction * Data.projectileSpeed;

            var damage = projectile.AddComponent<ProjectileDamage>();
            damage.Initialize(Data.damage);
        }
    }
}
