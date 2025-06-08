using System;
using R3;
using SpawningSpheresGame.Game.Common.DataTypes;
using SpawningSpheresGame.Game.Settings.DataStorage;
using SpawningSpheresGame.Game.State.Entities.Cameras;
using Zenject;

namespace MVVMZoomCamera
{
    public class ZoomRtsCameraModel<TEntity, TConfig> : IDisposable, IModel
        where TEntity : IZoomRtsCameraEntity<ZoomRtsCameraEntityData>
        where TConfig : ZoomRtsCameraConfig
    {
        private readonly CompositeDisposable _subscriptions = new();

        public DiContainer Container { get; }
        public TEntity Entity { get; }
        public TConfig Config { get; }

        public ZoomRtsCameraModel(TEntity entity, TConfig config, DiContainer container)
        {
            Entity = entity;
            Config = config;
            Container = container;
        }

        public void Dispose()
        {
            _subscriptions.Dispose();
            Entity.Dispose();
        }

        public void OnDestroyModel()
        {
            Dispose();
        }
    }

    public class ZoomRtsCameraModel : ZoomRtsCameraModel<ZoomRtsCameraEntity, ZoomRtsCameraConfig>
    {
        public ZoomRtsCameraModel(ZoomRtsCameraEntity entity, ZoomRtsCameraConfig config, DiContainer container)
            : base(entity, config, container) { }
    }
}