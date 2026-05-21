using UnityEngine;

namespace PortfolioFilling.Defense
{
    public sealed class RepairSystem : MonoBehaviour
    {
        [SerializeField] private int repairCost = 5;
        [SerializeField] private float repairAmount = 25f;

        public bool TryRepair(Barricade barricade, ResourceSystem resources)
        {
            if (barricade == null || resources == null || !resources.SpendScrap(repairCost))
            {
                return false;
            }

            barricade.Repair(repairAmount);
            return true;
        }
    }
}
