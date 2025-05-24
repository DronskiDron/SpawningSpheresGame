using SpawningSpheresGame.Game.Settings.DataStorage;
using SpawningSpheresGame.Game.State.Entities.Creatures.Player;

namespace PlayerController.MVVM.Player
{
    public class AdvancedPlayerBinder<TEntity, TConfig> : PlayerBinder<TEntity, TConfig>
        where TEntity : IAdvancedPlayerEntity<AdvancedPlayerEntityData>
        where TConfig : AdvancedPlayerConfig
    { }


    public class AdvancedPlayerBinder : AdvancedPlayerBinder<AdvancedPlayerEntity, AdvancedPlayerConfig> { }
}