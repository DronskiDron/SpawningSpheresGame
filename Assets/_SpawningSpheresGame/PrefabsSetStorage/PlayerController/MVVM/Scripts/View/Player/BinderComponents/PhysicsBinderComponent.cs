using PlayerController.MVVM.Player;
using SpawningSpheresGame.Game.Settings.DataStorage;
using SpawningSpheresGame.Game.State.Entities.Creatures.Player;
using UnityEngine;
using R3;
using SpawningSpheresGame.Game.Common.DataTypes;

namespace PlayerController.MVVM.View.BinderComponents
{
    public class PhysicsBinderComponent<TEntity, TConfig> : MVVMComponent
        where TEntity : IPlayerEntity<PlayerEntityData>
        where TConfig : PlayerConfig
    {
        private readonly Rigidbody _rigidbody;
        private readonly PlayerViewModel<TEntity, TConfig> _viewModel;

        private PlayerGravityApplier _gravityApplier;
        private PlayerJumpApplier _jumpApplier;
        private PlayerMovementApplier _movementApplier;


        public PhysicsBinderComponent(Rigidbody rigidbody, PlayerViewModel<TEntity, TConfig> viewModel)
        {
            _rigidbody = rigidbody;
            _viewModel = viewModel;
        }


        public override void Initialize()
        {
            _gravityApplier = new PlayerGravityApplier(_rigidbody);
            _jumpApplier = new PlayerJumpApplier(_rigidbody);
            _movementApplier = new PlayerMovementApplier(_rigidbody);
        }


        public void BindToFixedTick()
        {
            _viewModel.OnFixedTick.Subscribe(_ =>
            {
                var velocityChange = _viewModel.PlayerMovementCalculator.CalculateVelocityChange(
                    _viewModel.Input.MoveDirection,
                    _viewModel.Input.IsSprinting,
                    _rigidbody.rotation,
                    _rigidbody.velocity,
                    Time.fixedDeltaTime);

                var gravityForce = _viewModel.PlayerGravityCalculator.CalculateGravityForce(
                    _viewModel.Input.IsGrounded);

                var (shouldJump, jumpForce) = _viewModel.PlayerJumpCalculator.CalculateJump(
                    _viewModel.Input.IsJumping,
                    _viewModel.Input.IsGrounded,
                    Time.fixedDeltaTime);

                _movementApplier.ApplyMovement(velocityChange);
                _gravityApplier.ApplyGravity(gravityForce);

                if (shouldJump)
                {
                    _jumpApplier.ApplyJump(shouldJump, jumpForce);
                }

                _viewModel.UpdateTransformState(_rigidbody.position, _rigidbody.rotation);
            }).AddTo(Subscriptions);
        }
    }
}