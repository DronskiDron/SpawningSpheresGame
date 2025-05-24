namespace SpawningSpheresGame.Game.State.Entities.Cameras
{
    public interface IMoveRtsCameraEntity<out TData> : IEntity<TData>
        where TData : MoveRtsCameraEntityData
    {

    }

    public interface IMoveRtsCameraEntity : IEntity, IEntity<MoveRtsCameraEntityData> { }
}