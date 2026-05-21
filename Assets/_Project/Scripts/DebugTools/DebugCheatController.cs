using PortfolioFilling.Core;
using PortfolioFilling.Defense;
using PortfolioFilling.Enemy;
using PortfolioFilling.Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PortfolioFilling.DebugTools
{
    public sealed class DebugCheatController : MonoBehaviour
    {
        private void Update()
        {
            var keyboard = Keyboard.current;
            if (keyboard == null || GameRegistry.Instance == null)
            {
                return;
            }

            if (keyboard.f1Key.wasPressedThisFrame)
            {
                DebugLog.Toggle();
            }

            if (keyboard.f2Key.wasPressedThisFrame)
            {
                GameRegistry.Instance.Get<WaveManager>()?.SpawnSingleTestZombie();
            }

            if (keyboard.f3Key.wasPressedThisFrame)
            {
                GameRegistry.Instance.Get<PlayerHealth>()?.TakeDamage(10f);
            }

            if (keyboard.f4Key.wasPressedThisFrame)
            {
                GameRegistry.Instance.Get<PlayerWeaponController>()?.RefillAmmo();
                GameRegistry.Instance.Get<ResourceSystem>()?.AddScrap(25);
            }

            if (keyboard.f5Key.wasPressedThisFrame)
            {
                GameRegistry.Instance.Get<WaveManager>()?.StartNextWave();
            }
        }
    }
}
