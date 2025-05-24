using R3;
using SpawningSpheresGame.Game.Common.DataTypes;
using SpawningSpheresGame.Game.Settings.DataStorage;
using SpawningSpheresGame.Game.State.Entities.Cameras;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MVVMCameraMovement
{
    public class MoveRtsCameraInputBinderComponent : MVVMComponent
    {
        private readonly ApplicationInputController _inputActions;
        private readonly MoveRtsCameraViewModel<MoveRtsCameraEntity, MoveRtsCameraConfig> _viewModel;
        private readonly Camera _camera;
        private readonly LayerMask _raycastMask;
        private readonly LayerMask _groundMask;

        public MoveRtsCameraInputBinderComponent(
            ApplicationInputController inputActions,
            MoveRtsCameraViewModel<MoveRtsCameraEntity, MoveRtsCameraConfig> viewModel,
            Camera camera,
            LayerMask raycastMask,
            LayerMask groundMask)
        {
            _inputActions = inputActions;
            _viewModel = viewModel;
            _camera = camera;
            _raycastMask = raycastMask;
            _groundMask = groundMask;
        }

        public override void Initialize()
        {
            if (_inputActions == null) return;

            SubscribeToInput();
            _viewModel.OnTick.Subscribe(_ => OnUpdate()).AddTo(Subscriptions);
        }

        private void SubscribeToInput()
        {
            _inputActions.Camera.Drag.performed += OnDragPerformed;
            _inputActions.Camera.Drag.canceled += OnDragCanceled;
            _inputActions.Camera.Movement.performed += OnMovementPerformed;
            _inputActions.Camera.Movement.canceled += OnMovementCanceled;
            _inputActions.Camera.Rotate.performed += OnRotatePerformed;
            _inputActions.Camera.Rotate.canceled += OnRotateCanceled;
        }

        private void UnsubscribeFromInput()
        {
            if (_inputActions == null) return;
            _inputActions.Camera.Drag.performed -= OnDragPerformed;
            _inputActions.Camera.Drag.canceled -= OnDragCanceled;
            _inputActions.Camera.Movement.performed -= OnMovementPerformed;
            _inputActions.Camera.Movement.canceled -= OnMovementCanceled;
            _inputActions.Camera.Rotate.performed -= OnRotatePerformed;
            _inputActions.Camera.Rotate.canceled -= OnRotateCanceled;
        }

        private void OnUpdate()
        {
            if (_viewModel != null && Pointer.current != null)
            {
                _viewModel.MoveRtsCameraMovementComponent?.UpdateMousePosition(Pointer.current.position.ReadValue());
            }
        }

        private void OnRotatePerformed(InputAction.CallbackContext context)
        {
            float rotationInput = context.ReadValue<float>();
            _viewModel.MoveRtsCameraMovementComponent?.UpdateRotationInput(rotationInput);
        }

        private void OnRotateCanceled(InputAction.CallbackContext context)
        {
            _viewModel.MoveRtsCameraMovementComponent?.UpdateRotationInput(0);
        }

        private void OnDragPerformed(InputAction.CallbackContext context)
        {
            if (IsClickedOnGround())
            {
                _viewModel.MoveRtsCameraMovementComponent?.StartDrag(Pointer.current.position.ReadValue());
            }
        }

        private void OnDragCanceled(InputAction.CallbackContext context)
        {
            _viewModel.MoveRtsCameraMovementComponent?.StopDrag();
        }

        private void OnMovementPerformed(InputAction.CallbackContext context)
        {
            _viewModel.MoveRtsCameraMovementComponent?.UpdateKeyboardInput(context.ReadValue<Vector2>());
        }

        private void OnMovementCanceled(InputAction.CallbackContext context)
        {
            _viewModel.MoveRtsCameraMovementComponent?.UpdateKeyboardInput(Vector2.zero);
        }

        private bool IsClickedOnGround()
        {
            var pointerScreenPosition = Pointer.current.position.ReadValue();
            var ray = _camera.ScreenPointToRay(pointerScreenPosition);
            var result = Physics.Raycast(ray, out var hit, float.MaxValue, _raycastMask.value);
            if (!result) return false;

            return IsLayerInMask(hit.collider.gameObject.layer, _groundMask);
        }

        private bool IsLayerInMask(int layer, LayerMask mask)
        {
            return (mask.value & (1 << layer)) != 0;
        }

        public override void Dispose()
        {
            UnsubscribeFromInput();
            base.Dispose();
        }
    }
}