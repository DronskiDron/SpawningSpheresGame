using SpawningSpheresGame.Game.Settings.DataStorage;
using SpawningSpheresGame.Game.State.Entities.Creatures.Player;

namespace PlayerController.MVVM.Player
{
    public class AdvancedPlayerViewModel<TEntity, TConfig> : PlayerViewModel<TEntity, TConfig>
        where TEntity : IAdvancedPlayerEntity<AdvancedPlayerEntityData>
        where TConfig : AdvancedPlayerConfig
    {
        public AdvancedPlayerViewModel(AdvancedPlayerModel<TEntity, TConfig> model) : base(model) { }
    }


    public class AdvancedPlayerViewModel : AdvancedPlayerViewModel<AdvancedPlayerEntity, AdvancedPlayerConfig>
    {
        public AdvancedPlayerViewModel(AdvancedPlayerModel model) : base(model) { }
    }
}