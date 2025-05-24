namespace SpawningSpheresGame.Game.State.Entities
{
    public interface IEntity<out TData> : IEntityBase where TData : EntityData
    {
        TData Origin { get; }
    }

    public interface IEntity : IEntity<EntityData> { }
}