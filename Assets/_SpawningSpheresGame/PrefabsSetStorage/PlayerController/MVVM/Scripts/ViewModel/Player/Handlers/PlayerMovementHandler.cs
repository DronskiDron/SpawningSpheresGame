using PlayerController.Classic;
using SpawningSpheresGame.Game.State.DataTypes;
using UnityEngine;

namespace PlayerController.MVVM.Player
{
    public class PlayerMovementHandler : IMovementHandler
    {
        private readonly Rigidbody _rb;
        private readonly TransformState _transformState;
        private readonly CharacterSettings _settings;


        public PlayerMovementHandler(Rigidbody rb, TransformState transformState, CharacterSettings settings)
        {
            _rb = rb;
            _transformState = transformState;
            _settings = settings;
        }


        public void Move(Vector2 input, bool isSprinting)
        {
            float speed = isSprinting ? _settings.SprintSpeed : _settings.MoveSpeed;

            Vector3 moveDirection = (_transformState.Rotation.Value * new Vector3(input.x, 0, input.y)).normalized;

            Vector3 targetVelocity = moveDirection * speed;
            Vector3 velocityChange = targetVelocity - new Vector3(_rb.velocity.x, 0, _rb.velocity.z);

            velocityChange = Vector3.ClampMagnitude(velocityChange, _settings.MaxAcceleration * Time.fixedDeltaTime);

            _rb.AddForce(velocityChange, ForceMode.VelocityChange);

            _transformState.Position.Value = _rb.position;
            _transformState.Rotation.Value = _rb.rotation;
        }
    }
}