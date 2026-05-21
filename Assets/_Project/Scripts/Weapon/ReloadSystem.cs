using UnityEngine;

namespace PortfolioFilling.Weapon
{
    public sealed class ReloadSystem
    {
        private float _reloadEndsAt;

        public bool IsReloading => Time.time < _reloadEndsAt;

        public void Begin(float duration)
        {
            _reloadEndsAt = Time.time + duration;
        }

        public bool ConsumeFinished()
        {
            return _reloadEndsAt > 0f && Time.time >= _reloadEndsAt;
        }

        public void Clear()
        {
            _reloadEndsAt = 0f;
        }
    }
}
