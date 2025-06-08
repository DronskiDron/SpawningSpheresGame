namespace SpawningSpheresGame.Game.State.Entities.Creatures
{
    public abstract class CreatureEntity<TData> : Entity<TData>, ICreatureEntity<TData>
    where TData : CreatureEntityData
    {
        public int SpecificCreatureField { get; }
        protected CreatureEntity(TData data) : base(data) { }
    }

    public class CreatureEntity : CreatureEntity<CreatureEntityData>, ICreatureEntity
    {
        public CreatureEntity(CreatureEntityData data) : base(data) { }

        EntityData IEntity<EntityData>.Origin => Origin;
    }
}