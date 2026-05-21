using PortfolioFilling.Core;
using UnityEngine;

namespace PortfolioFilling.Weapon
{
    public abstract class WeaponBase : MonoBehaviour
    {
        private Transform _fireOrigin;
        private float _nextShotAt;

        protected WeaponData Data { get; private set; }
        protected AmmoSystem Ammo { get; private set; }
        protected ReloadSystem Reload { get; private set; }

        public void Configure(WeaponData data)
        {
            Data = data;
            Ammo = new AmmoSystem(data.clipSize, data.reserveAmmo);
            Reload = new ReloadSystem();
        }

        public void Initialize(Transform fireOrigin)
        {
            _fireOrigin = fireOrigin;
        }

        public void Tick()
        {
            if (Reload == null || Ammo == null || !Reload.ConsumeFinished())
            {
                return;
            }

            Ammo.ReloadFull();
            Reload.Clear();
            DebugLog.Log($"{Data.displayName} 장전 완료.", this);
        }

        public bool TryFire()
        {
            if (Data == null || Ammo == null || Reload == null || _fireOrigin == null)
            {
                return false;
            }

            if (Reload.IsReloading || Time.time < _nextShotAt || !Ammo.CanFire())
            {
                return false;
            }

            _nextShotAt = Time.time + Data.fireInterval;
            Ammo.ConsumeRound();
            FireShot(_fireOrigin.position, _fireOrigin.forward);
            return true;
        }

        public void TryStartReload()
        {
            if (Data == null || Ammo == null || Reload == null || Reload.IsReloading || !Ammo.CanReload())
            {
                return;
            }

            Reload.Begin(Data.reloadDuration);
            DebugLog.Log($"{Data.displayName} 장전 시작.", this);
        }

        public void RefillAmmo()
        {
            Ammo?.Refill();
        }

        public string GetDebugStatus()
        {
            return Data == null || Ammo == null
                ? "설정되지 않은 무기"
                : $"{Data.displayName} | {Ammo.CurrentClip}/{Ammo.CurrentReserve}" + (Reload.IsReloading ? " (장전 중)" : string.Empty);
        }

        protected abstract void FireShot(Vector3 origin, Vector3 direction);
    }
}
