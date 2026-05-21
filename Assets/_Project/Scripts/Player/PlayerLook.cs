using PortfolioFilling.Core;
using UnityEngine;

namespace PortfolioFilling.Player
{
    public sealed class PlayerLook : MonoBehaviour
    {
        private PlayerInputReader _input;
        private Transform _body;
        private Transform _cameraTransform;
        private HarnessGameConfig _config;
        private float _pitch;

        public void Initialize(PlayerInputReader input, Transform body, Transform cameraTransform, ConfigManager configManager)
        {
            _input = input;
            _body = body;
            _cameraTransform = cameraTransform;
            _config = configManager.Runtime;
        }

        private void Update()
        {
            if (_input == null || _body == null || _cameraTransform == null || _config == null)
            {
                return;
            }

            var lookDelta = _input.Look * _config.lookSensitivity;
            _pitch = Mathf.Clamp(_pitch - lookDelta.y, -80f, 80f);
            _cameraTransform.localRotation = Quaternion.Euler(_pitch, 0f, 0f);
            _body.Rotate(Vector3.up * lookDelta.x);
        }
    }
}
