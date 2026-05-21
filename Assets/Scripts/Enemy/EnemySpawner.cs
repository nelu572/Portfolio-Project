using PortfolioFilling.Core;
using PortfolioFilling.Defense;
using UnityEngine;

namespace PortfolioFilling.Enemy
{
    public sealed class EnemySpawner : MonoBehaviour
    {
        private Transform _player;
        private DefenseObjective _objective;
        private ConfigManager _configManager;
        private readonly Vector3[] _spawnPoints =
        {
            new(-12f, 1f, 14f),
            new(12f, 1f, 14f),
            new(0f, 1f, 16f)
        };

        public void Initialize(Transform player, DefenseObjective objective, ConfigManager configManager)
        {
            _player = player;
            _objective = objective;
            _configManager = configManager;
        }

        public EnemyBase SpawnZombie(int spawnIndex, WaveManager waveManager)
        {
            var zombieObject = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            zombieObject.name = $"Zombie_{spawnIndex}";
            zombieObject.transform.position = _spawnPoints[spawnIndex % _spawnPoints.Length];
            zombieObject.GetComponent<Renderer>().sharedMaterial.color = new Color(0.35f, 0.45f, 0.28f);

            var enemy = zombieObject.AddComponent<EnemyBase>();
            enemy.Initialize(_player, _objective, _configManager, waveManager);
            return enemy;
        }
    }
}
