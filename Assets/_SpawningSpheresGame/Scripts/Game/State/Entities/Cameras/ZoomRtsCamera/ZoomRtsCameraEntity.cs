using R3;

namespace SpawningSpheresGame.Game.State.Entities.Cameras
{
    public class ZoomRtsCameraEntity<TData> : Entity<TData>, IZoomRtsCameraEntity<TData>
        where TData : ZoomRtsCameraEntityData
    {
        public ReactiveProperty<float> FieldOfView { get; }
        public ReactiveProperty<float> OrthographicSize { get; }

        public ZoomRtsCameraEntity(TData data) : base(data)
        {
            FieldOfView = new ReactiveProperty<float>(data.FieldOfView);
            FieldOfView.Skip(1).Subscribe(value => data.FieldOfView = value);

            OrthographicSize = new ReactiveProperty<float>(data.OrthographicSize);
            OrthographicSize.Skip(1).Subscribe(value => data.OrthographicSize = value);
        }
    }

    public class ZoomRtsCameraEntity : ZoomRtsCameraEntity<ZoomRtsCameraEntityData>, IZoomRtsCameraEntity
    {
        public ZoomRtsCameraEntity(ZoomRtsCameraEntityData data) : base(data) { }

        EntityData IEntity<EntityData>.Origin => Origin;
    }
}