namespace SpawningSpheresGame.Game.State.Entities.Creatures
{
    public interface ICreatureEntity<out TData> : IEntity<TData> where TData : CreatureEntityData
    {
        int SpecificCreatureField { get; }
    }

    public interface ICreatureEntity : IEntity, IEntity<CreatureEntityData> { }
}