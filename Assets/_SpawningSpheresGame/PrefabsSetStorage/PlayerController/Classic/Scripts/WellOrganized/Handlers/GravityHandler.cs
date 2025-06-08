using UnityEngine;

namespace PlayerController.Classic
{
    public class GravityHandler : IGravityHandler
    {
        private readonly Rigidbody _rb;
        private readonly CharacterSettings _settings;


        public GravityHandler(Rigidbody rb, CharacterSettings settings)
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