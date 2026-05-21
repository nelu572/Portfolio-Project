using System.Collections.Generic;
using PortfolioFilling.Core;
using UnityEngine;

namespace PortfolioFilling.Enemy
{
    public sealed class WaveManager : MonoBehaviour
    {
        private readonly List<EnemyBase> _aliveEnemies = new();
        private EnemySpawner _spawner;

        public int CurrentWave { get; private set; }
        public bool WaveInProgress => _aliveEnemies.Count > 0;
        public int AliveCount => _aliveEnemies.Count;

        public void Initialize(EnemySpawner spawner)
        {
            _spawner = spawner;
        }

        public void StartNextWave()
        {
            if (_spawner == null || WaveInProgress)
            {
                return;
            }

            CurrentWave++;
            var spawnCount = Mathf.Max(1, CurrentWave * 2);
            _aliveEnemies.Clear();

            for (var i = 0; i < spawnCount; i++)
            {
                _aliveEnemies.Add(_spawner.SpawnZombie(i, this));
            }

            GameEventBus.PublishWaveStarted(CurrentWave);
        }

        public void SpawnSingleTestZombie()
        {
            if (_spawner == null)
            {
                return;
            }

            _aliveEnemies.Add(_spawner.SpawnZombie(_aliveEnemies.Count, this));
        }

        public void NotifyEnemyKilled(EnemyBase enemy)
        {
            _aliveEnemies.Remove(enemy);
            GameEventBus.PublishEnemyKilled();

            if (_aliveEnemies.Count == 0 && CurrentWave > 0)
            {
                GameEventBus.PublishWaveCompleted(CurrentWave);
            }
        }
    }
}
