using UnityEngine;

namespace PortfolioFilling.Core
{
    public sealed class GameManager : MonoBehaviour
    {
        public bool IsPaused { get; private set; }

        public void SetPaused(bool paused)
        {
            IsPaused = paused;
            Time.timeScale = paused ? 0f : 1f;
        }
    }
}
