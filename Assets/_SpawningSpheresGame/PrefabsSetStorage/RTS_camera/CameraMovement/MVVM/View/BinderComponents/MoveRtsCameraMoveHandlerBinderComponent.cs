using SpawningSpheresGame.Game.Common.DataTypes;
using SpawningSpheresGame.Game.Settings.DataStorage;
using SpawningSpheresGame.Game.State.Entities.Cameras;
using UnityEngine;

namespace MVVMCameraMovement
{
    public class MoveRtsCameraMoveHandlerBinderComponent : MVVMComponent
    {
        private readonly MoveRtsCameraViewModel<MoveRtsCameraEntity, MoveRtsCameraConfig> _viewModel;
        private readonly Camera _camera;

        public MoveRtsCameraMoveHandlerBinderComponent(
            MoveRtsCameraViewModel<MoveRtsCameraEntity, MoveRtsCameraConfig> viewModel,
            Camera camera)
        {
            _viewModel = viewModel;
            _camera = camera;
        }

        public override void Initialize()
        {
            var presetId = SelectPresetId();
            var moveHandler = SelectMoveHandler(presetId);
            InitMoveHandler(moveHandler);
        }

        private CameraPresetId SelectPresetId()
        {
            var presetId = _camera.orthographic ? CameraPresetId.Orthographic : CameraPresetId.Perspective;
            return presetId;
        }

        private IRtsCameraMovementHandler SelectMoveHandler(CameraPresetId presetId)
        {
            var config = _viewModel.GetConfig();
            var cameraPreset = config.GetCameraPresetById(presetId);
            _viewModel.SetCameraPreset(cameraPreset);
            var movementProperties = cameraPreset.CameraMovementProperties;

            return new SmoothCameraMovementHandler(_viewModel.TransformSynchronizer, movementProperties);
        }

        private void InitMoveHandler(IRtsCameraMovementHandler moveHandler)
        {
            _viewModel.InitMoveHandler(moveHandler);
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}