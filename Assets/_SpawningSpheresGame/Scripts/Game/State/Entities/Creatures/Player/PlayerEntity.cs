using SpawningSpheresGame.Game.State.DataTypes;

namespace SpawningSpheresGame.Game.State.Entities.Creatures.Player
{
    public abstract class PlayerEntity<TData> : CreatureEntity<TData>, IPlayerEntity<TData>
        where TData : PlayerEntityData
    {
        public TransformState PlayerCameraOffset { get; }

        protected PlayerEntity(TData data) : base(data)
        {
            PlayerCameraOffset = new TransformState(data.PlayerCameraOffsetData);
        }

        public override void Dispose()
        {
            base.Dispose();
            PlayerCameraOffset.Dispose();
        }
    }

    public class PlayerEntity : PlayerEntity<PlayerEntityData>, IPlayerEntity
    {
        public PlayerEntity(PlayerEntityData data) : base(data) { }

        CreatureEntityData IEntity<CreatureEntityData>.Origin => Origin;

        EntityData IEntity<EntityData>.Origin => Origin;
    }
}