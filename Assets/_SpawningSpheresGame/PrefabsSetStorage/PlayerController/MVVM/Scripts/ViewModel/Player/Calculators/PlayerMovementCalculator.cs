using PlayerController.Classic;
using UnityEngine;

namespace PlayerController.MVVM.Player
{
    public class PlayerMovementCalculator
    {
        private readonly CharacterSettings _settings;


        public PlayerMovementCalculator(CharacterSettings settings)
        {
            _settings = settings;
        }


        public Vector3 CalculateVelocityChange(Vector2 input, bool isSprinting, Quaternion rotation, Vector3 currentVelocity, float deltaTime)
        {
            float speed = isSprinting ? _settings.SprintSpeed : _settings.MoveSpeed;
            Vector3 moveDirection = (rotation * new Vector3(input.x, 0, input.y)).normalized;
            Vector3 targetVelocity = moveDirection * speed;
            Vector3 velocityChange = targetVelocity - new Vector3(currentVelocity.x, 0, currentVelocity.z);
            return Vector3.ClampMagnitude(velocityChange, _settings.MaxAcceleration * deltaTime);
        }
    }
}