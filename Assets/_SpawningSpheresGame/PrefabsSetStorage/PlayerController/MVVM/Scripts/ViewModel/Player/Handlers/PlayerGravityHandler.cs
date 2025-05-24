using PlayerController.Classic;
using UnityEngine;

namespace PlayerController.MVVM.Player
{
    public class PlayerGravityHandler : IGravityHandler
    {
        private readonly Rigidbody _rb;
        private readonly CharacterSettings _settings;


        public PlayerGravityHandler(Rigidbody rb, CharacterSettings settings)
        {
            _rb = rb;
            _settings = settings;
        }


        public void ApplyGravity(bool isGrounded)
        {
            if (!isGrounded)
            {
                _rb.AddForce(Vector3.down * _settings.GravityMultiplier, ForceMode.Acceleration);
            }
        }
    }
}