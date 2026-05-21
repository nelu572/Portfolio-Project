using PortfolioFilling.Core;
using UnityEngine;

namespace PortfolioFilling.Player
{
    public sealed class PlayerMovement : MonoBehaviour
    {
        private PlayerInputReader _input;
        private CharacterController _controller;
        private HarnessGameConfig _config;
        private Vector3 _velocity;

        public void Initialize(PlayerInputReader input, CharacterController controller, ConfigManager configManager)
        {
            _input = input;
            _controller = controller;
            _config = configManager.Runtime;
        }

        private void Update()
        {
            if (_input == null || _controller == null || _config == null)
            {
                return;
            }

            var moveInput = _input.Move;
            var move = transform.right * moveInput.x + transform.forward * moveInput.y;
            var speed = _input.SprintHeld ? _config.sprintSpeed : _config.walkSpeed;
            _controller.Move(move * speed * Time.deltaTime);

            if (_controller.isGrounded && _velocity.y < 0f)
            {
                _velocity.y = -2f;
            }

            if (_controller.isGrounded && _input.ConsumeJumpPressed())
            {
                _velocity.y = Mathf.Sqrt(_config.jumpHeight * -2f * _config.gravity);
            }

            _velocity.y += _config.gravity * Time.deltaTime;
            _controller.Move(_velocity * Time.deltaTime);
        }
    }
}
