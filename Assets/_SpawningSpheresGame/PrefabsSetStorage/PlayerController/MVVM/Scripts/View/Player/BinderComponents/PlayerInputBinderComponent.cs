using SpawningSpheresGame.Game.Common.DataTypes;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerController.MVVM.View.BinderComponents
{
    public class PlayerInputBinderComponent : MVVMComponent
    {
        private readonly ApplicationInputController _inputActions;
        private readonly CreatureInput _creatureInput;

        public PlayerInputBinderComponent(ApplicationInputController inputActions, CreatureInput creatureInput)
        {
            _inputActions = inputActions;
            _creatureInput = creatureInput;
        }

        public override void Initialize()
        {
            if (_inputActions == null) return;

            SubscribeToInput();
        }

        private void SubscribeToInput()
        {
            _inputActions.Player.Move.performed += OnMovePerformed;
            _inputActions.Player.Move.canceled += OnMoveCanceled;

            _inputActions.Player.Look.performed += OnLookPerformed;
            _inputActions.Player.Look.canceled += OnLookCanceled;

            _inputActions.Player.Jump.started += OnJumpStarted;
            _inputActions.Player.Jump.canceled += OnJumpCanceled;

            _inputActions.Player.Sprint.started += OnSprintStarted;
            _inputActions.Player.Sprint.canceled += OnSprintCanceled;
        }

        private void UnsubscribeFromInput()
        {
            if (_inputActions == null) return;

            _inputActions.Player.Move.performed -= OnMovePerformed;
            _inputActions.Player.Move.canceled -= OnMoveCanceled;

            _inputActions.Player.Look.performed -= OnLookPerformed;
            _inputActions.Player.Look.canceled -= OnLookCanceled;

            _inputActions.Player.Jump.started -= OnJumpStarted;
            _inputActions.Player.Jump.canceled -= OnJumpCanceled;

            _inputActions.Player.Sprint.started -= OnSprintStarted;
            _inputActions.Player.Sprint.canceled -= OnSprintCanceled;
        }

        private void OnMovePerformed(InputAction.CallbackContext ctx) => _creatureInput.MoveDirection = ctx.ReadValue<Vector2>();
        private void OnMoveCanceled(InputAction.CallbackContext ctx) => _creatureInput.MoveDirection = Vector2.zero;

        private void OnLookPerformed(InputAction.CallbackContext ctx) => _creatureInput.LookDirection = ctx.ReadValue<Vector2>();
        private void OnLookCanceled(InputAction.CallbackContext ctx) => _creatureInput.LookDirection = Vector2.zero;

        private void OnJumpStarted(InputAction.CallbackContext ctx) => _creatureInput.IsJumping = true;
        private void OnJumpCanceled(InputAction.CallbackContext ctx) => _creatureInput.IsJumping = false;

        private void OnSprintStarted(InputAction.CallbackContext ctx) => _creatureInput.IsSprinting = true;
        private void OnSprintCanceled(InputAction.CallbackContext ctx) => _creatureInput.IsSprinting = false;

        public override void Dispose()
        {
            UnsubscribeFromInput();
            base.Dispose();
        }
    }
}