using SpawningSpheresGame.Game.State.DataTypes;
using UnityEngine;

namespace MVVMCameraMovement
{
    public class SmoothCameraMovementHandler : IRtsCameraMovementHandler
    {
        private readonly TransformSynchronizer _transformSync;
        private readonly CameraMovementProperties _properties;
        private Vector3 _cachedCameraPosition;

        public SmoothCameraMovementHandler(TransformSynchronizer transformSync, CameraMovementProperties properties)
        {
            _transformSync = transformSync;
            _properties = properties;
            _cachedCameraPosition = transformSync.Position.Value;
        }

        public void Move(Vector3 inputDelta)
        {
            _cachedCameraPosition += inputDelta * _properties.Speed;

            _transformSync.Position.Value = Vector3.Lerp(
                _transformSync.Position.Value, _cachedCameraPosition, Time.deltaTime / _properties.Smoothness);
        }
    }
}