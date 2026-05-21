using UnityEngine;

namespace PortfolioFilling.Core
{
    public sealed class TimeManager : MonoBehaviour
    {
        public void SetTimeScale(float scale)
        {
            Time.timeScale = Mathf.Max(0f, scale);
        }
    }
}
