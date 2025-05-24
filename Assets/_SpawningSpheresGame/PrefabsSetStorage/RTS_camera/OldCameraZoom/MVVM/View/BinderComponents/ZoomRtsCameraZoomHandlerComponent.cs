using SpawningSpheresGame.Game.Common.DataTypes;
using SpawningSpheresGame.Game.Settings.DataStorage;
using SpawningSpheresGame.Game.State.Entities.Cameras;
using UnityEngine;

namespace MVVMZoomCamera.View.BinderComponents
{
    public class ZoomRtsCameraZoomHandlerComponent : MVVMComponent
    {
        private readonly ZoomRtsCameraViewModel<ZoomRtsCameraEntity, ZoomRtsCameraConfig> _viewModel;
        private Camera _camera;

        public ZoomRtsCameraZoomHandlerComponent(ZoomRtsCameraViewModel<ZoomRtsCameraEntity, ZoomRtsCameraConfig> viewModel
                                                , Camera camera)
        {
            _viewModel = viewModel;
            _camera = camera;
        }

        public override void Initialize()
        {
            var presetId = SelectPresetId();
            var zoomHandler = SelectZoomHandler(presetId);
            InitZoomHandler(zoomHandler);
        }

        private CameraPresetId SelectPresetId()
        {
            var presetId = _camera.orthographic ? CameraPresetId.Orthographic : CameraPresetId.Perspective;

            return presetId;
        }

        private IZoomHandler SelectZoomHandler(CameraPresetId presetId)
        {
            var zoomProperties = _viewModel.GetAndInitZoomProperties(presetId);
            var entity = _viewModel.GetEntity();

            IZoomHandler zoomHandler = presetId == CameraPresetId.Orthographic
                ? new OrthographicCameraZoomHandler(entity.OrthographicSize, zoomProperties)
                : new PerspectiveCameraZoomHandler(entity.FieldOfView, zoomProperties);
            return zoomHandler;
        }

        private void InitZoomHandler(IZoomHandler zoomHandler)
        {
            _viewModel.InitializeZoomHandler(zoomHandler);
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}