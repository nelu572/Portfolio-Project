using System;

namespace PortfolioFilling.Weapon
{
    [Serializable]
    public sealed class WeaponData
    {
        public string displayName = "무기";
        public float damage = 10f;
        public float fireInterval = 0.2f;
        public int clipSize = 8;
        public int reserveAmmo = 32;
        public float reloadDuration = 1.5f;
        public float range = 50f;
        public float projectileSpeed = 20f;
    }
}
