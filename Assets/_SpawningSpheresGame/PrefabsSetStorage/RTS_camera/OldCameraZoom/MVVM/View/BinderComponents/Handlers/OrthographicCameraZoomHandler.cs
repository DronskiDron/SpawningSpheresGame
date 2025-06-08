using R3;
using SpawningSpheresGame.Game.State.Entities.Cameras;
using UnityEngine;

namespace MVVMZoomCamera.View.BinderComponents
{
    public class OrthographicCameraZoomHandler : IZoomHandler
    {
        private readonly ReactiveProperty<float> _orthographicSize;
        private readonly ZoomProperties _properties;
        private float _orthoSize;
        private float _velocity;

        public OrthographicCameraZoomHandler(ReactiveProperty<float> orthographicSize
                                            , ZoomProperties properties)
        {
            _orthographicSize = orthographicSize;
            _properties = properties;
            _orthoSize = orthographicSize.Value;
        }

        public void Zoom(float inputDelta)
        {
            var inputDeltaWithSpeed = inputDelta * _properties.ZoomSpeed;

            _orthoSize = Mathf.Clamp(_orthoSize - inputDeltaWithSpeed, _properties.ZoomMin, _properties.ZoomMax);

            var newOrthoSize = Mathf.SmoothDamp(
                _orthographicSize.Value,
                _orthoSize,
                ref _velocity,
                _properties.Smoothness);

            _orthographicSize.Value = newOrthoSize;
        }
    }
}