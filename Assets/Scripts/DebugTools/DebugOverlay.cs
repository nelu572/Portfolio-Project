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
            GUILayout.Label("하네스 디버그");
            GUILayout.Label($"FPS: {1f / Mathf.Max(0.0001f, _smoothedDeltaTime):0}");
            GUILayout.Label($"로그: {(DebugLog.Enabled ? "켜짐" : "꺼짐")} | F1");
            GUILayout.Label($"플레이어 체력: {playerHealth?.CurrentHealth ?? 0f:0}/{playerHealth?.MaxHealth ?? 0f:0}");
            GUILayout.Label($"무기: {weaponController?.GetWeaponStatus() ?? "없음"}");
            GUILayout.Label($"웨이브: {waveManager?.CurrentWave ?? 0} | 생존 적: {waveManager?.AliveCount ?? 0}");
            GUILayout.Label($"목표물 체력: {objective?.CurrentHealth ?? 0f:0}/{objective?.MaxHealth ?? 0f:0}");
            GUILayout.Label($"바리케이드 체력: {objective?.Barricade?.CurrentHealth ?? 0f:0}/{objective?.Barricade?.MaxHealth ?? 0f:0}");
            GUILayout.Label($"스크랩: {_resourceSystem?.Scrap ?? 0}");

            if (GUILayout.Button("웨이브 시작"))
            {
                waveManager?.StartNextWave();
            }

            if (GUILayout.Button("테스트 좀비 생성"))
            {
                waveManager?.SpawnSingleTestZombie();
            }

            if (GUILayout.Button("로그 토글"))
            {
                DebugLog.Toggle();
            }

            GUILayout.Label("F2 좀비 생성 | F3 플레이어 피해 | F4 탄약 보충 | F5 웨이브 시작");
            GUILayout.EndArea();
        }
    }
}
