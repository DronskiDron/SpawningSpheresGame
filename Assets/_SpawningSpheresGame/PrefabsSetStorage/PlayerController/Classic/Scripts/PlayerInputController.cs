using SpawningSpheresGame.Game.Settings.Controlls;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerController.Classic
{
    public class PlayerInputController : MonoBehaviour
    {
        public Vector2 MoveInput { get; private set; }
        public Vector2 LookInput { get; private set; }
        public bool IsJumping { get; private set; }
        public bool IsSprinting { get; private set; }

        private ApplicationInputController _inputActions;


        private void Awake()
        {
            _inputActions = new ApplicationInputController();

            var inputManager = new InputSchemeManager(_inputActions.asset);
            inputManager.AutoDetectScheme();
        }


        private void OnEnable()
        {
            _inputActions.Enable();

            _inputActions.Player.Move.performed += OnMovePerformed;
            _inputActions.Player.Move.canceled += OnMoveCanceled;

            _inputActions.Player.Look.performed += OnLookPerformed;
            _inputActions.Player.Look.canceled += OnLookCanceled;

            _inputActions.Player.Jump.started += OnJumpStarted;
            _inputActions.Player.Jump.canceled += OnJumpCanceled;

            _inputActions.Player.Sprint.started += OnSprintStarted;
            _inputActions.Player.Sprint.canceled += OnSprintCanceled;
        }


        private void OnDisable()
        {
            _inputActions.Player.Move.performed -= OnMovePerformed;
            _inputActions.Player.Move.canceled -= OnMoveCanceled;

            _inputActions.Player.Look.performed -= OnLookPerformed;
            _inputActions.Player.Look.canceled -= OnLookCanceled;

            _inputActions.Player.Jump.started -= OnJumpStarted;
            _inputActions.Player.Jump.canceled -= OnJumpCanceled;

            _inputActions.Player.Sprint.started -= OnSprintStarted;
            _inputActions.Player.Sprint.canceled -= OnSprintCanceled;

            _inputActions.Disable();
        }


        private void OnMovePerformed(InputAction.CallbackContext ctx) => MoveInput = ctx.ReadValue<Vector2>();
        private void OnMoveCanceled(InputAction.CallbackContext ctx) => MoveInput = Vector2.zero;

        private void OnLookPerformed(InputAction.CallbackContext ctx) => LookInput = ctx.ReadValue<Vector2>();
        private void OnLookCanceled(InputAction.CallbackContext ctx) => LookInput = Vector2.zero;

        private void OnJumpStarted(InputAction.CallbackContext ctx) => IsJumping = true;
        private void OnJumpCanceled(InputAction.CallbackContext ctx) => IsJumping = false;

        private void OnSprintStarted(InputAction.CallbackContext ctx) => IsSprinting = true;
        private void OnSprintCanceled(InputAction.CallbackContext ctx) => IsSprinting = false;
    }
}
