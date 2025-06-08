using PlayerController.MVVM.Player;
using SpawningSpheresGame.Game.Settings.DataStorage;
using SpawningSpheresGame.Game.State.Entities.Creatures.Player;
using UnityEngine;
using R3;
using SpawningSpheresGame.Game.Common.DataTypes;

namespace PlayerController.MVVM.View.BinderComponents
{
    public class CameraBinderComponent<TEntity, TConfig> : MVVMComponent
        where TEntity : IPlayerEntity<PlayerEntityData>
        where TConfig : PlayerConfig
    {
        private readonly Transform _cameraPivot;
        private readonly PlayerViewModel<TEntity, TConfig> _viewModel;
        private PlayerLookApplier _lookApplier;
        private readonly Rigidbody _rigidbody;


        public CameraBinderComponent(Transform cameraPivot, PlayerViewModel<TEntity, TConfig> viewModel, Rigidbody rigidbody)
        {
            _cameraPivot = cameraPivot;
            _viewModel = viewModel;
            _rigidbody = rigidbody;
        }


        public override void Initialize()
        {
            _lookApplier = new PlayerLookApplier(_rigidbody, _cameraPivot);
            _viewModel.CameraRotation.Subscribe(newValue => _cameraPivot.localRotation = newValue).AddTo(Subscriptions);
        }

        public void BindToTick()
        {
            _viewModel.OnTick.Subscribe(_ =>
            {
                var horizontalRotation = _viewModel.PlayerLookCalculator.
                    CalculateHorizontalRotation(_viewModel.Input.LookDirection);
                var verticalRotation = _viewModel.PlayerLookCalculator.
                    CalculateVerticalRotation(_viewModel.Input.LookDirection);
                _lookApplier.ApplyLook(horizontalRotation, verticalRotation);
            }).AddTo(Subscriptions);
        }
    }
}