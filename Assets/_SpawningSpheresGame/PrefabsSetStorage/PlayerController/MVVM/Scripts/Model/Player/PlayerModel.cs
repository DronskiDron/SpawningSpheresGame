using SpawningSpheresGame.Game.Settings.DataStorage;
using SpawningSpheresGame.Game.State.Entities.Creatures.Player;
using Zenject;

namespace PlayerController.MVVM.Player
{
    public class PlayerModel<TEntity, TConfig> : CreatureModel<TEntity, TConfig>
        where TEntity : IPlayerEntity<PlayerEntityData>
        where TConfig : PlayerConfig
    {
        public PlayerModel(TEntity creatureEntity, TConfig config, DiContainer container)
            : base(creatureEntity, config, container) { }
    }


    public class PlayerModel : PlayerModel<PlayerEntity, PlayerConfig>
    {
        public PlayerModel(PlayerEntity creatureEntity, PlayerConfig config, DiContainer container)
            : base(creatureEntity, config, container) { }
    }
}