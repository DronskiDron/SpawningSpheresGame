using ObservableCollections;
using R3;

namespace SpawningSpheresGame.Game.State.Entities.Creatures.Player
{
    public interface IAdvancedPlayerEntity<out TData> : IPlayerEntity<TData>
        where TData : AdvancedPlayerEntityData
    {
        ReactiveProperty<float> SpecialPower { get; }
        ReactiveProperty<int> ExtraLives { get; }
        ObservableList<string> Abilities { get; }
    }

    public interface IAdvancedPlayerEntity : IPlayerEntity, IAdvancedPlayerEntity<AdvancedPlayerEntityData> { }
}