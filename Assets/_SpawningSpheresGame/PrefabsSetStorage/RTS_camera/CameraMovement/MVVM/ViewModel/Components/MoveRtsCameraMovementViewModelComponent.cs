using R3;
using SpawningSpheresGame.Game.Common.DataTypes;
using SpawningSpheresGame.Game.Settings.DataStorage;
using SpawningSpheresGame.Game.State.Entities.Cameras;
using UnityEngine;

namespace MVVMCameraMovement
{
    public class MoveRtsCameraMovementViewModelComponent : MVVMComponent
    {
        private readonly RtsCameraMovementLocalProperties _localMovementProperties = new();
        private readonly MoveRtsCameraViewModel<MoveRtsCameraEntity, MoveRtsCameraConfig> _viewModel;

        private IRtsCameraMovementHandler _movementHandler;

        public MoveRtsCameraMovementViewModelComponent(MoveRtsCameraViewModel<MoveRtsCameraEntity, MoveRtsCameraConfig> viewModel)
        {
            _viewModel = viewModel;
        }

        public override void Initialize()
        {
            var currentConfig = _viewModel.CameraPreset;
            _localMovementProperties.MouseSensitivity = currentConfig.CameraMovementProperties.MouseSensitivity;
            _localMovementProperties.KeyboardSensitivity = currentConfig.CameraMovementProperties.KeyboardSensitivity;
            _localMovementProperties.RotationSpeed = currentConfig.CameraMovementProperties.RotationSpeed;

            _viewModel.OnTick.Subscribe(_ => OnTick()).AddTo(Subscriptions);
            _viewModel.OnTick.Subscribe(_ => _viewModel.TransformSynchronizer.CheckForTransformChanges()).AddTo(Subscriptions);
        }

        public void InitializeMovementHandler(IRtsCameraMovementHandler movementHandler)
        {
            _movementHandler = movementHandler;
        }

        public void StartDrag(Vector3 initialMousePosition)
        {
            _localMovementProperties.DragEnabled = true;
            _localMovementProperties.PreviousMousePosition = initialMousePosition;
        }

        public void StopDrag()
        {
            _localMovementProperties.DragEnabled = false;
            _localMovementProperties.MouseDelta = Vector3.zero;
        }

        public void UpdateMousePosition(Vector3 currentMousePosition)
        {
            if (_localMovementProperties.DragEnabled)
            {
                _localMovementProperties.MouseDelta = (currentMousePosition - _localMovementProperties.PreviousMousePosition)
                    * _localMovementProperties.MouseSensitivity;
                _localMovementProperties.PreviousMousePosition = currentMousePosition;
            }
        }

        public void UpdateKeyboardInput(Vector2 keyboardInput)
        {
            _localMovementProperties.KeyboardInput = keyboardInput * _localMovementProperties.KeyboardSensitivity;
        }

        public void UpdateRotationInput(float value)
        {
            _localMovementProperties.RotationInput = value;
        }

        protected void OnTick()
        {
            if (_localMovementProperties.RotationInput != 0)
            {
                Rotate();
            }

            Move();
        }

        private void Rotate()
        {
            var rotation = _viewModel.TransformSynchronizer.Rotation;
            Quaternion currentRotation = rotation.Value;
            float yRotation = _localMovementProperties.RotationInput
                * _localMovementProperties.RotationSpeed * Time.deltaTime;

            Quaternion newRotation = Quaternion.AngleAxis(yRotation, Vector3.up) * currentRotation;
            rotation.Value = Quaternion.Euler(currentRotation.eulerAngles.x, newRotation.eulerAngles.y, currentRotation.eulerAngles.z);
        }

        private void Move()
        {
            float yaw = _viewModel.TransformSynchronizer.Rotation.Value.eulerAngles.y;
            Quaternion yawRotation = Quaternion.Euler(0, yaw, 0);

            Vector3 forward = yawRotation * Vector3.forward;
            Vector3 right = yawRotation * Vector3.right;

            Vector3 movementDelta = forward * _localMovementProperties.KeyboardInput.y + right
                * _localMovementProperties.KeyboardInput.x;

            Vector3 mouseMovement = yawRotation * new Vector3(-_localMovementProperties.MouseDelta.x, 0
                , -_localMovementProperties.MouseDelta.y);
            movementDelta += mouseMovement;

            _movementHandler.Move(movementDelta);
        }

        public override void Dispose()
        {
            _movementHandler = null;
            base.Dispose();
        }
    }
}