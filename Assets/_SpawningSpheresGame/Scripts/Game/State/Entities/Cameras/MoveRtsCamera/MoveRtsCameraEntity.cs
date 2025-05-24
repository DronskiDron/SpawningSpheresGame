namespace SpawningSpheresGame.Game.State.Entities.Cameras
{
    public class MoveRtsCameraEntity<TData> : Entity<TData>, IMoveRtsCameraEntity<TData>
        where TData : MoveRtsCameraEntityData
    {
        public MoveRtsCameraEntity(TData data) : base(data) { }
    }

    public class MoveRtsCameraEntity : MoveRtsCameraEntity<MoveRtsCameraEntityData>, IMoveRtsCameraEntity
    {
        public MoveRtsCameraEntity(MoveRtsCameraEntityData data) : base(data) { }

        EntityData IEntity<EntityData>.Origin => Origin;
    }
}