using UnityEngine;

namespace PlayerController.MVVM.Player
{
    public class PlayerMovementApplier
    {
        private readonly Rigidbody _rb;


        public PlayerMovementApplier(Rigidbody rb)
        {
            _rb = rb;
        }


        public void ApplyMovement(Vector3 velocityChange)
        {
            _rb.AddForce(velocityChange, ForceMode.VelocityChange);
        }
    }
}