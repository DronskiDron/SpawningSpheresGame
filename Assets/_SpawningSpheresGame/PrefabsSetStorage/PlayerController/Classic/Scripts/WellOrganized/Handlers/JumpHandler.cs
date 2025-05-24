using UnityEngine;

namespace PlayerController.Classic
{
    public class JumpHandler : IJumpHandler
    {
        private readonly Rigidbody _rb;
        private readonly CharacterSettings _settings;

        private float _jumpTimeCounter;
        private bool _isJumping;


        public JumpHandler(Rigidbody rb, CharacterSettings settings)
        {
            _rb = rb;
            _settings = settings;
        }


        public void HandleJump(bool jumpPressed, bool isGrounded)
        {
            if (isGrounded && jumpPressed)
            {
                StartJump();
            }

            if (_isJumping)
            {
                if (jumpPressed && _jumpTimeCounter > 0)
                {
                    _rb.velocity = new Vector3(_rb.velocity.x, _settings.JumpHoldForce, _rb.velocity.z);
                    _jumpTimeCounter -= Time.deltaTime;
                }
                else
                {
                    _isJumping = false;
                }
            }
        }


        private void StartJump()
        {
            _isJumping = true;
            _jumpTimeCounter = _settings.MaxJumpTime;
            _rb.velocity = new Vector3(_rb.velocity.x, _settings.JumpForce, _rb.velocity.z);
        }
    }
}