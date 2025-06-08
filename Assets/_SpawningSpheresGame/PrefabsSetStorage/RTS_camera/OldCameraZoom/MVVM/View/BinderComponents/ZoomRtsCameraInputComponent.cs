using SpawningSpheresGame.Game.Common.DataTypes;
using SpawningSpheresGame.Game.Settings.DataStorage;
using SpawningSpheresGame.Game.State.Entities.Cameras;
using UnityEngine.InputSystem;

namespace MVVMZoomCamera.View.BinderComponents
{
    public class ZoomRtsCameraInputComponent : MVVMComponent
    {
        private readonly ApplicationInputController _inputActions;
        private readonly ZoomRtsCameraViewModel<ZoomRtsCameraEntity, ZoomRtsCameraConfig> _viewModel;

        public ZoomRtsCameraInputComponent(ApplicationInputController inputActions
            , ZoomRtsCameraViewModel<ZoomRtsCameraEntity, ZoomRtsCameraConfig> viewModel)
        {
            _inputActions = inputActions;
            _viewModel = viewModel;
        }

        public override void Initialize()
        {
            if (_inputActions == null) return;

            SubscribeToInput();
        }

        private void SubscribeToInput()
        {
            _inputActions.Camera.Zoom.performed += OnZoomPerformed;
            _inputActions.Camera.Zoom.canceled += OnZoomCanceled;
        }

        private void UnsubscribeFromInput()
        {
            if (_inputActions == null) return;
            _inputActions.Camera.Zoom.performed -= OnZoomPerformed;
            _inputActions.Camera.Zoom.canceled -= OnZoomCanceled;
        }

        private void OnZoomPerformed(InputAction.CallbackContext context)
        {
            _viewModel?.ReadInputDelta(context.ReadValue<float>());
        }

        private void OnZoomCanceled(InputAction.CallbackContext context)
        {
            _viewModel?.ReadInputDelta(0f);
        }

        public override void Dispose()
        {
            UnsubscribeFromInput();
            base.Dispose();
        }
    }
}