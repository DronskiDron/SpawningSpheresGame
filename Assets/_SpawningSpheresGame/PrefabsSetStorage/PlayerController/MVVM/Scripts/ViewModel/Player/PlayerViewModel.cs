using SpawningSpheresGame.Game.Settings.DataStorage;
using SpawningSpheresGame.Game.State.Entities.Creatures.Player;
using R3;
using UnityEngine;

namespace PlayerController.MVVM.Player
{
    public class PlayerViewModel<TEntity, TConfig> : CreatureViewModel<TEntity, TConfig>
        where TEntity : IPlayerEntity<PlayerEntityData>
        where TConfig : PlayerConfig
    {
        public PlayerGravityCalculator PlayerGravityCalculator { get; protected set; }
        public PlayerJumpCalculator PlayerJumpCalculator { get; protected set; }
        public PlayerLookCalculator PlayerLookCalculator { get; protected set; }
        public PlayerMovementCalculator PlayerMovementCalculator { get; protected set; }

        public ReactiveProperty<Vector3> CameraPosition = new();
        public ReactiveProperty<Quaternion> CameraRotation = new();


        public PlayerViewModel(PlayerModel<TEntity, TConfig> model) : base(model) { }


        public ApplicationInputController GetApplicationInputController()
        {
            var applicationInputController = Model.Container.Resolve<ApplicationInputController>();
            return applicationInputController;
        }


        public override void InitHandlers()
        {
            var characterSettings = Model.Config.CharacterSettings;
            PlayerGravityCalculator = new PlayerGravityCalculator(characterSettings);
            PlayerJumpCalculator = new PlayerJumpCalculator(characterSettings);
            PlayerLookCalculator = new PlayerLookCalculator(characterSettings);
            PlayerMovementCalculator = new PlayerMovementCalculator(characterSettings);
        }


        protected override void BindToModel(CreatureModel<TEntity, TConfig> model)
        {
            base.BindToModel(model);

            if (model.CreatureEntity.PlayerCameraOffset != null)
            {
                model.CreatureEntity.PlayerCameraOffset.Position
                    .Subscribe(newValue => CameraPosition.Value = newValue)
                    .AddTo(Subscriptions);

                model.CreatureEntity.PlayerCameraOffset.Rotation
                    .Subscribe(newValue => CameraRotation.Value = newValue)
                    .AddTo(Subscriptions);
            }
        }


        public override void UpdateTransformState(Vector3 position, Quaternion rotation)
        {
            base.UpdateTransformState(position, rotation);

            if (Model.CreatureEntity.PlayerCameraOffset != null)
            {
                Model.CreatureEntity.PlayerCameraOffset.Position.Value = position;
                Model.CreatureEntity.PlayerCameraOffset.Rotation.Value = rotation;
            }
        }
    }


    public class PlayerViewModel : PlayerViewModel<PlayerEntity, PlayerConfig>
    {
        public PlayerViewModel(PlayerModel<PlayerEntity, PlayerConfig> model) : base(model) { }
    }
}