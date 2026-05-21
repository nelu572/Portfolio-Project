using UnityEngine;

namespace PortfolioFilling.Core
{
    public static class DebugLog
    {
        public static bool Enabled { get; private set; } = true;

        public static void SetEnabled(bool enabled)
        {
            Enabled = enabled;
            GameEventBus.PublishDebugLogsChanged(enabled);
        }

        public static void Toggle()
        {
            SetEnabled(!Enabled);
        }

        public static void Log(string message, Object context = null)
        {
            if (!Enabled)
            {
                return;
            }

            Debug.Log(message, context);
        }
    }
}
