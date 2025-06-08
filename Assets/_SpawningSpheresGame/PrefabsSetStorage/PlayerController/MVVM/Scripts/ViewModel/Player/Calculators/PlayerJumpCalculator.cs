using PlayerController.Classic;

namespace PlayerController.MVVM.Player
{
    public class PlayerJumpCalculator
    {
        private readonly CharacterSettings _settings;
        private float _jumpTimeCounter;
        private bool _isJumping;


        public PlayerJumpCalculator(CharacterSettings settings)
        {
            _settings = settings;
        }


        public (bool, float) CalculateJump(bool jumpPressed, bool isGrounded, float deltaTime)
        {
            if (isGrounded && jumpPressed)
            {
                StartJump();
            }

            if (_isJumping)
            {
                if (jumpPressed && _jumpTimeCounter > 0)
                {
                    _jumpTimeCounter -= deltaTime;
                    return (true, _settings.JumpHoldForce);
                }
                else
                {
                    _isJumping = false;
                }
            }

            return (false, 0f);
        }


        private void StartJump()
        {
            _isJumping = true;
            _jumpTimeCounter = _settings.MaxJumpTime;
        }


        public float GetInitialJumpForce()
        {
            return _settings.JumpForce;
        }
    }
}