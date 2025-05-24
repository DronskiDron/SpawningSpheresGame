using SpawningSpheresGame.Game.Settings.DataStorage;
using SpawningSpheresGame.Game.State.Entities.Creatures.Player;
using Zenject;

namespace PlayerController.MVVM.Player
{
    public class AdvancedPlayerModel<TEntity, TConfig> : PlayerModel<TEntity, TConfig>
        where TEntity : IAdvancedPlayerEntity<AdvancedPlayerEntityData>
        where TConfig : AdvancedPlayerConfig
    {
        public AdvancedPlayerModel(TEntity creatureEntity, TConfig config, DiContainer container)
            : base(creatureEntity, config, container) { }
    }


    public class AdvancedPlayerModel : AdvancedPlayerModel<AdvancedPlayerEntity, AdvancedPlayerConfig>
    {
        public AdvancedPlayerModel(AdvancedPlayerEntity creatureEntity, AdvancedPlayerConfig config, DiContainer container)
            : base(creatureEntity, config, container) { }
    }
}