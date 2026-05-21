using UnityEngine;
using UnityEngine.InputSystem;

namespace PortfolioFilling.Player
{
    public sealed class PlayerInputReader : MonoBehaviour
    {
        private InputActionMap _map;
        private InputAction _moveAction;
        private InputAction _lookAction;
        private InputAction _attackAction;
        private InputAction _interactAction;
        private InputAction _jumpAction;
        private InputAction _sprintAction;
        private InputAction _reloadAction;
        private InputAction _previousWeaponAction;
        private InputAction _nextWeaponAction;

        private bool _jumpPressed;
        private bool _interactPressed;
        private bool _reloadPressed;
        private bool _previousWeaponPressed;
        private bool _nextWeaponPressed;

        public Vector2 Move => _moveAction.ReadValue<Vector2>();
        public Vector2 Look => _lookAction.ReadValue<Vector2>();
        public bool AttackHeld => _attackAction.IsPressed();
        public bool SprintHeld => _sprintAction.IsPressed();

        private void Awake()
        {
            _map = new InputActionMap("HarnessPlayer");

            _moveAction = _map.AddAction("Move");
            _moveAction.AddCompositeBinding("2DVector")
                .With("Up", "<Keyboard>/w")
                .With("Down", "<Keyboard>/s")
                .With("Left", "<Keyboard>/a")
                .With("Right", "<Keyboard>/d");
            _moveAction.AddBinding("<Gamepad>/leftStick");

            _lookAction = _map.AddAction("Look");
            _lookAction.AddBinding("<Mouse>/delta");
            _lookAction.AddBinding("<Gamepad>/rightStick");

            _attackAction = _map.AddAction("Attack");
            _attackAction.AddBinding("<Mouse>/leftButton");
            _attackAction.AddBinding("<Gamepad>/rightTrigger");

            _interactAction = _map.AddAction("Interact");
            _interactAction.AddBinding("<Keyboard>/e");
            _interactAction.performed += _ => _interactPressed = true;

            _jumpAction = _map.AddAction("Jump");
            _jumpAction.AddBinding("<Keyboard>/space");
            _jumpAction.performed += _ => _jumpPressed = true;

            _sprintAction = _map.AddAction("Sprint");
            _sprintAction.AddBinding("<Keyboard>/leftShift");

            _reloadAction = _map.AddAction("Reload");
            _reloadAction.AddBinding("<Keyboard>/r");
            _reloadAction.performed += _ => _reloadPressed = true;

            _previousWeaponAction = _map.AddAction("PreviousWeapon");
            _previousWeaponAction.AddBinding("<Keyboard>/1");
            _previousWeaponAction.performed += _ => _previousWeaponPressed = true;

            _nextWeaponAction = _map.AddAction("NextWeapon");
            _nextWeaponAction.AddBinding("<Keyboard>/2");
            _nextWeaponAction.performed += _ => _nextWeaponPressed = true;
        }

        private void OnEnable()
        {
            _map.Enable();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void OnDisable()
        {
            _map.Disable();
        }

        public bool ConsumeJumpPressed()
        {
            var value = _jumpPressed;
            _jumpPressed = false;
            return value;
        }

        public bool ConsumeInteractPressed()
        {
            var value = _interactPressed;
            _interactPressed = false;
            return value;
        }

        public bool ConsumeReloadPressed()
        {
            var value = _reloadPressed;
            _reloadPressed = false;
            return value;
        }

        public bool ConsumePreviousWeaponPressed()
        {
            var value = _previousWeaponPressed;
            _previousWeaponPressed = false;
            return value;
        }

        public bool ConsumeNextWeaponPressed()
        {
            var value = _nextWeaponPressed;
            _nextWeaponPressed = false;
            return value;
        }
    }
}
