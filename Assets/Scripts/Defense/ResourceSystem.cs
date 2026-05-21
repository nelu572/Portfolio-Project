using UnityEngine;

namespace PortfolioFilling.Defense
{
    public sealed class ResourceSystem : MonoBehaviour
    {
        public int Scrap { get; private set; }

        public void SetScrap(int amount)
        {
            Scrap = Mathf.Max(0, amount);
        }

        public void AddScrap(int amount)
        {
            Scrap += Mathf.Max(0, amount);
        }

        public bool SpendScrap(int amount)
        {
            if (amount <= 0 || Scrap < amount)
            {
                return false;
            }

            Scrap -= amount;
            return true;
        }
    }
}
