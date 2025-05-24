using SpawningSpheresGame.Game.Settings.DataStorage;
using SpawningSpheresGame.Game.State.Entities.Creatures.Player;
using UnityEngine;
using PlayerController.MVVM.View.BinderComponents;

namespace PlayerController.MVVM.Player
{
    public class PlayerBinder<TEntity, TConfig> : CreatureBinder<TEntity, TConfig>
        where TEntity : IPlayerEntity<PlayerEntityData>
        where TConfig : PlayerConfig
    {
        [SerializeField] protected Transform CameraPivot;

        protected PlayerInputBinderComponent InputComponent;
        protected PhysicsBinderComponent<TEntity, TConfig> PhysicsComponent;
        protected CameraBinderComponent<TEntity, TConfig> CameraComponent;

        public override void Init(CreatureViewModel<TEntity, TConfig> viewModel)
        {
            base.Init(viewModel);

            if (viewModel is PlayerViewModel<TEntity, TConfig> playerViewModel)
            {
                InputComponent = new PlayerInputBinderComponent(
                    playerViewModel.GetApplicationInputController(),
                    playerViewModel.Input);

                PhysicsComponent = new PhysicsBinderComponent<TEntity, TConfig>(
                    Rb,
                    playerViewModel);

                CameraComponent = new CameraBinderComponent<TEntity, TConfig>(
                    CameraPivot,
                    playerViewModel,
                    Rb);

                InputComponent.Initialize();
                PhysicsComponent.Initialize();
                CameraComponent.Initialize();

                PhysicsComponent.BindToFixedTick();
                CameraComponent.BindToTick();
            }
        }

        public override void Dispose()
        {
            InputComponent?.Dispose();
            PhysicsComponent?.Dispose();
            CameraComponent?.Dispose();
            base.Dispose();
        }


        public override void OnDestroyView()
        {
            base.OnDestroyView();
        }
    }

    public class PlayerBinder : PlayerBinder<PlayerEntity, PlayerConfig> { }
}