using PortfolioFilling.Core.Interfaces;
using UnityEngine;

namespace PortfolioFilling.Defense
{
    public sealed class DefenseObjective : MonoBehaviour, IDamageable, IInteractable
    {
        [SerializeField] private float maxHealth = 150f;

        private Barricade _barricade;

        public float CurrentHealth { get; private set; }
        public float MaxHealth => maxHealth;
        public Barricade Barricade => _barricade;

        private void Awake()
        {
            CurrentHealth = maxHealth;
        }

        public void SetBarricade(Barricade barricade)
        {
            _barricade = barricade;
        }

        public void TakeDamage(float amount)
        {
            if (_barricade != null && _barricade.CurrentHealth > 0f)
            {
                _barricade.TakeDamage(amount);
                return;
            }

            CurrentHealth = Mathf.Max(0f, CurrentHealth - amount);
        }

        public void Interact()
        {
            if (_barricade != null)
            {
                _barricade.Repair(5f);
            }
        }
    }
}
