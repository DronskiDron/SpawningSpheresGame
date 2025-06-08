using UnityEngine;

namespace PlayerController.MVVM.Player
{
    public class PlayerJumpApplier
    {
        private readonly Rigidbody _rb;


        public PlayerJumpApplier(Rigidbody rb)
        {
            _rb = rb;
        }


        public void ApplyJump(bool shouldJump, float jumpForce)
        {
            if (shouldJump)
            {
                _rb.velocity = new Vector3(_rb.velocity.x, jumpForce, _rb.velocity.z);
            }
        }


        public void StartJump(float initialJumpForce)
        {
            _rb.velocity = new Vector3(_rb.velocity.x, initialJumpForce, _rb.velocity.z);
        }
    }
}