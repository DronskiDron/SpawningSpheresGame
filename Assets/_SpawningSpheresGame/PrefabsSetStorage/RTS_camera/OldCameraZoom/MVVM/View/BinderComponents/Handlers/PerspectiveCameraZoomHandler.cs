using R3;
using UnityEngine;

namespace MVVMZoomCamera.View.BinderComponents
{
    public class PerspectiveCameraZoomHandler : IZoomHandler
    {
        private readonly ReactiveProperty<float> _fieldOfView;
        private readonly ZoomProperties _properties;
        private float _fov;
        private float _velocity;

        public PerspectiveCameraZoomHandler(ReactiveProperty<float> fieldOfView, ZoomProperties properties)
        {
            _fieldOfView = fieldOfView;
            _properties = properties;
            _fov = fieldOfView.Value;
        }

        public void Zoom(float inputDelta)
        {
            var inputDeltaWithSpeed = inputDelta * _properties.ZoomSpeed;

            _fov = Mathf.Clamp(_fov - inputDeltaWithSpeed, _properties.ZoomMin, _properties.ZoomMax);

            var newFov = Mathf.SmoothDamp(
                _fieldOfView.Value,
                _fov,
                ref _velocity,
                _properties.Smoothness);

            _fieldOfView.Value = newFov;
        }
    }
}