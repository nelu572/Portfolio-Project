using System;

namespace PortfolioFilling.Core
{
    public static class GameEventBus
    {
        public static event Action<int> WaveStarted;
        public static event Action<int> WaveCompleted;
        public static event Action EnemyKilled;
        public static event Action<bool> DebugLogsChanged;

        public static void PublishWaveStarted(int waveNumber) => WaveStarted?.Invoke(waveNumber);
        public static void PublishWaveCompleted(int waveNumber) => WaveCompleted?.Invoke(waveNumber);
        public static void PublishEnemyKilled() => EnemyKilled?.Invoke();
        public static void PublishDebugLogsChanged(bool enabled) => DebugLogsChanged?.Invoke(enabled);
    }
}
