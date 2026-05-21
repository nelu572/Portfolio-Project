using PortfolioFilling.Core;
using PortfolioFilling.Defense;
using PortfolioFilling.Enemy;
using PortfolioFilling.Player;
using UnityEngine;

namespace PortfolioFilling.DebugTools
{
    public sealed class DebugOverlay : MonoBehaviour
    {
        private ConfigManager _configManager;
        private ResourceSystem _resourceSystem;
        private float _smoothedDeltaTime;

        public void Initialize(ConfigManager configManager, ResourceSystem resourceSystem)
        {
            _configManager = configManager;
            _resourceSystem = resourceSystem;
        }

        private void Update()
        {
            _smoothedDeltaTime += (Time.unscaledDeltaTime - _smoothedDeltaTime) * 0.1f;
        }

        private void OnGUI()
        {
            var registry = GameRegistry.Instance;
            if (registry == null)
            {
                return;
            }

            var waveManager = registry.Get<WaveManager>();
            var playerHealth = registry.Get<PlayerHealth>();
            var weaponController = registry.Get<PlayerWeaponController>();
            var objective = registry.Get<DefenseObjective>();

            GUILayout.BeginArea(new Rect(16f, 16f, 330f, 260f), GUI.skin.box);
            GUILayout.Label("Harness Debug");
            GUILayout.Label($"FPS: {1f / Mathf.Max(0.0001f, _smoothedDeltaTime):0}");
            GUILayout.Label($"Logs: {(DebugLog.Enabled ? "On" : "Off")} | F1");
            GUILayout.Label($"Player HP: {playerHealth?.CurrentHealth ?? 0f:0}/{playerHealth?.MaxHealth ?? 0f:0}");
            GUILayout.Label($"Weapon: {weaponController?.GetWeaponStatus() ?? "None"}");
            GUILayout.Label($"Wave: {waveManager?.CurrentWave ?? 0} | Alive: {waveManager?.AliveCount ?? 0}");
            GUILayout.Label($"Objective HP: {objective?.CurrentHealth ?? 0f:0}/{objective?.MaxHealth ?? 0f:0}");
            GUILayout.Label($"Barricade HP: {objective?.Barricade?.CurrentHealth ?? 0f:0}/{objective?.Barricade?.MaxHealth ?? 0f:0}");
            GUILayout.Label($"Scrap: {_resourceSystem?.Scrap ?? 0}");

            if (GUILayout.Button("Start Wave"))
            {
                waveManager?.StartNextWave();
            }

            if (GUILayout.Button("Spawn Test Zombie"))
            {
                waveManager?.SpawnSingleTestZombie();
            }

            if (GUILayout.Button("Toggle Logs"))
            {
                DebugLog.Toggle();
            }

            GUILayout.Label("F2 Spawn Zombie | F3 Damage Player | F4 Refill Ammo | F5 Start Wave");
            GUILayout.EndArea();
        }
    }
}
