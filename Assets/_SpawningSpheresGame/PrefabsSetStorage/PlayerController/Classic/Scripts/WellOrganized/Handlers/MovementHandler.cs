using UnityEngine;

namespace PlayerController.Classic
{
    public class MovementHandler : IMovementHandler
    {
        private readonly Rigidbody _rb;
        private readonly Transform _playerBody;
        private readonly CharacterSettings _settings;


        public MovementHandler(Rigidbody rb, Transform playerBody, CharacterSettings settings)
        {
            _rb = rb;
            _playerBody = playerBody;
            _settings = settings;
        }


        public void Move(Vector2 input, bool isSprinting)
        {
            float speed = isSprinting ? _settings.SprintSpeed : _settings.MoveSpeed;

            Vector3 moveDirection = (_playerBody.rotation * new Vector3(input.x, 0, input.y)).normalized;
            Vector3 targetVelocity = moveDirection * speed;
            Vector3 velocityChange = targetVelocity - new Vector3(_rb.velocity.x, 0, _rb.velocity.z);

            velocityChange = Vector3.ClampMagnitude(velocityChange, _settings.MaxAcceleration * Time.fixedDeltaTime);

            _rb.AddForce(velocityChange, ForceMode.VelocityChange);
        }
    }
}