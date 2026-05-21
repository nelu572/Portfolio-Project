using UnityEngine;

namespace PortfolioFilling.Defense
{
    public class BuildableObject : MonoBehaviour
    {
        [SerializeField] private int buildCost = 10;

        public virtual bool TryBuild(ResourceSystem resources)
        {
            return resources != null && resources.SpendScrap(buildCost);
        }
    }
}
