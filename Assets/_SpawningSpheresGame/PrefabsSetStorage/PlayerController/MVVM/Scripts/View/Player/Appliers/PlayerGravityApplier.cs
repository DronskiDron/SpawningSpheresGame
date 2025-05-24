using UnityEngine;

namespace PlayerController.MVVM.Player
{
    public class PlayerGravityApplier
    {
        private readonly Rigidbody _rb;


        public PlayerGravityApplier(Rigidbody rb)
        {
            _rb = rb;
        }


        public void ApplyGravity(Vector3 gravityForce)
        {
            if (gravityForce != Vector3.zero)
            {
                _rb.AddForce(gravityForce, ForceMode.Acceleration);
            }
        }
    }
}