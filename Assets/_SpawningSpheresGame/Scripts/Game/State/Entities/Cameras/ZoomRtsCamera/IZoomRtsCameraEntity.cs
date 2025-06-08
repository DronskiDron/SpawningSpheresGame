using R3;

namespace SpawningSpheresGame.Game.State.Entities.Cameras
{
    public interface IZoomRtsCameraEntity<out TData> : IEntity<TData>
        where TData : ZoomRtsCameraEntityData
    {
        ReactiveProperty<float> FieldOfView { get; }
        ReactiveProperty<float> OrthographicSize { get; }
    }

    public interface IZoomRtsCameraEntity : IEntity, IEntity<ZoomRtsCameraEntityData> { }
}