using PlayerController.Classic;
using SpawningSpheresGame.Game.State.DataTypes;
using UnityEngine;

namespace PlayerController.MVVM.Player
{
    public class PlayerLookHandler : ILookHandler
    {
        private readonly Rigidbody _rb;
        private readonly TransformState _cameraPivot;
        private readonly CharacterSettings _settings;

        private float _rotationX = 0f;


        public PlayerLookHandler(Rigidbody rb, TransformState cameraPivot, CharacterSettings settings)
        {
            _rb = rb;
            _cameraPivot = cameraPivot;
            _settings = settings;
        }


        public void HandleLook(Vector2 lookInput)
        {
            Quaternion horizontalRotation = Quaternion.Euler(0, lookInput.x * _settings.LookSpeedX, 0);
            _rb.MoveRotation(_rb.rotation * horizontalRotation);

            _rotationX -= lookInput.y * _settings.LookSpeedY;
            _rotationX = Mathf.Clamp(_rotationX, _settings.LowerLookLimit, _settings.UpperLookLimit);
            _cameraPivot.Rotation.Value = Quaternion.Euler(_rotationX, 0, 0);
        }
    }
}