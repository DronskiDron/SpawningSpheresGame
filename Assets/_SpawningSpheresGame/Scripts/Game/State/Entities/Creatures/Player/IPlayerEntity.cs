using SpawningSpheresGame.Game.State.DataTypes;

namespace SpawningSpheresGame.Game.State.Entities.Creatures.Player
{
    public interface IPlayerEntity<out TData> : ICreatureEntity<TData> where TData : PlayerEntityData
    {
        TransformState PlayerCameraOffset { get; }
    }

    public interface IPlayerEntity : ICreatureEntity, ICreatureEntity<PlayerEntityData> { }
}