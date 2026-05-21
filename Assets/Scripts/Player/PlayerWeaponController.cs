using System.Collections.Generic;
using PortfolioFilling.Weapon;
using UnityEngine;

namespace PortfolioFilling.Player
{
    public sealed class PlayerWeaponController : MonoBehaviour
    {
        private readonly List<WeaponBase> _weapons = new();
        private PlayerInputReader _input;
        private Transform _fireOrigin;
        private int _currentIndex;

        public WeaponBase CurrentWeapon => _weapons.Count == 0 ? null : _weapons[_currentIndex];

        public void Initialize(PlayerInputReader input, Transform fireOrigin)
        {
            _input = input;
            _fireOrigin = fireOrigin;
        }

        public void AddWeapon(WeaponBase weapon)
        {
            weapon.transform.SetParent(transform);
            weapon.Initialize(_fireOrigin);
            _weapons.Add(weapon);
            RefreshWeaponStates();
        }

        private void Update()
        {
            if (_input == null || CurrentWeapon == null)
            {
                return;
            }

            if (_input.ConsumePreviousWeaponPressed())
            {
                SwitchWeapon(-1);
            }

            if (_input.ConsumeNextWeaponPressed())
            {
                SwitchWeapon(1);
            }

            if (_input.ConsumeReloadPressed())
            {
                CurrentWeapon.TryStartReload();
            }

            CurrentWeapon.Tick();

            if (_input.AttackHeld)
            {
                CurrentWeapon.TryFire();
            }
        }

        public void RefillAmmo()
        {
            CurrentWeapon?.RefillAmmo();
        }

        public string GetWeaponStatus()
        {
            return CurrentWeapon == null ? "무기 없음" : CurrentWeapon.GetDebugStatus();
        }

        private void SwitchWeapon(int direction)
        {
            if (_weapons.Count <= 1)
            {
                return;
            }

            _currentIndex = (_currentIndex + direction + _weapons.Count) % _weapons.Count;
            RefreshWeaponStates();
        }

        private void RefreshWeaponStates()
        {
            for (var i = 0; i < _weapons.Count; i++)
            {
                _weapons[i].gameObject.SetActive(i == _currentIndex);
            }
        }
    }
}
