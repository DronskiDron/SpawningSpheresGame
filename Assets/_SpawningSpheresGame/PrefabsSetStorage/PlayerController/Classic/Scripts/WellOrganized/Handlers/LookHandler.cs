using UnityEngine;

namespace PlayerController.Classic
{
    public class LookHandler : ILookHandler
    {
        private readonly Transform _cameraPivot;
        private readonly Transform _playerBody;
        private readonly CharacterSettings _settings;

        private float _rotationX = 0f;


        public LookHandler(Transform cameraPivot, Transform playerBody, CharacterSettings settings)
        {
            _cameraPivot = cameraPivot;
            _playerBody = playerBody;
            _settings = settings;
        }


        public void HandleLook(Vector2 lookInput)
        {
            _playerBody.Rotate(0, lookInput.x * _settings.LookSpeedX, 0);
            _rotationX -= lookInput.y * _settings.LookSpeedY;
            _rotationX = Mathf.Clamp(_rotationX, _settings.LowerLookLimit, _settings.UpperLookLimit);
            _cameraPivot.localRotation = Quaternion.Euler(_rotationX, 0, 0);
        }
    }
}