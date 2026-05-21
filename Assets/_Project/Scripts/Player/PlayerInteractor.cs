using PortfolioFilling.Core.Interfaces;
using UnityEngine;

namespace PortfolioFilling.Player
{
    public sealed class PlayerInteractor : MonoBehaviour
    {
        private PlayerInputReader _input;
        private Transform _cameraTransform;
        private float _range;

        public void Initialize(PlayerInputReader input, Transform cameraTransform, float range)
        {
            _input = input;
            _cameraTransform = cameraTransform;
            _range = range;
        }

        private void Update()
        {
            if (_input == null || _cameraTransform == null || !_input.ConsumeInteractPressed())
            {
                return;
            }

            if (!Physics.Raycast(_cameraTransform.position, _cameraTransform.forward, out var hit, _range))
            {
                return;
            }

            if (hit.collider.TryGetComponent<IInteractable>(out var interactable))
            {
                interactable.Interact();
            }
        }
    }
}
