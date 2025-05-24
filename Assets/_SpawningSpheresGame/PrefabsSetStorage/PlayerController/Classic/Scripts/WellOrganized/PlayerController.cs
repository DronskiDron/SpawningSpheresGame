using UnityEngine;

namespace PlayerController.Classic
{
    public class PlayerController : MonoBehaviour
    {
        public Rigidbody _rb;
        public GroundCheck _groundCheck;
        public Transform _cameraPivot;
        public Transform _playerBody;
        public PlayerInputController _inputController;
        public CharacterSettings _settings;

        private IMovementHandler _movementHandler;
        private IJumpHandler _jumpHandler;
        private IGravityHandler _gravityHandler;
        private ILookHandler _lookHandler;


        private void Awake()
        {
            _movementHandler = new MovementHandler(_rb, _playerBody, _settings);
            _jumpHandler = new JumpHandler(_rb, _settings);
            _gravityHandler = new GravityHandler(_rb, _settings);
            _lookHandler = new LookHandler(_cameraPivot, _playerBody, _settings);
        }


        private void Update()
        {
            _lookHandler.HandleLook(_inputController.LookInput);
            _jumpHandler.HandleJump(_inputController.IsJumping, _groundCheck.IsGrounded);
        }


        private void FixedUpdate()
        {
            _movementHandler.Move(_inputController.MoveInput, _inputController.IsSprinting);
            _gravityHandler.ApplyGravity(_groundCheck.IsGrounded);
        }
    }
}