using PlayerController.Classic;
using UnityEngine;

namespace PlayerController.MVVM.Player
{
    public class PlayerLookCalculator
    {
        private readonly CharacterSettings _settings;
        private float _rotationX = 0f;


        public PlayerLookCalculator(CharacterSettings settings)
        {
            _settings = settings;
        }


        public Quaternion CalculateHorizontalRotation(Vector2 lookInput)
        {
            return Quaternion.Euler(0, lookInput.x * _settings.LookSpeedX, 0);
        }


        public Quaternion CalculateVerticalRotation(Vector2 lookInput)
        {
            _rotationX -= lookInput.y * _settings.LookSpeedY;
            _rotationX = Mathf.Clamp(_rotationX, _settings.LowerLookLimit, _settings.UpperLookLimit);
            return Quaternion.Euler(_rotationX, 0, 0);
        }
    }
}