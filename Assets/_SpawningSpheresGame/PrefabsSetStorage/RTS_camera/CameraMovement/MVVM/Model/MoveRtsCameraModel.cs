using System;
using R3;
using SpawningSpheresGame.Game.Common.DataTypes;
using SpawningSpheresGame.Game.Settings.DataStorage;
using SpawningSpheresGame.Game.State.Entities.Cameras;
using Zenject;

namespace MVVMCameraMovement
{
    public class MoveRtsCameraModel<TEntity, TConfig> : IDisposable, IModel
        where TEntity : IMoveRtsCameraEntity<MoveRtsCameraEntityData>
        where TConfig : MoveRtsCameraConfig
    {
        private readonly CompositeDisposable _subscriptions = new();

        public DiContainer Container { get; }
        public TEntity Entity { get; }
        public TConfig Config { get; }

        public MoveRtsCameraModel(TEntity entity, TConfig config, DiContainer container)
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

    public class MoveRtsCameraModel : MoveRtsCameraModel<MoveRtsCameraEntity, MoveRtsCameraConfig>
    {
        public MoveRtsCameraModel(MoveRtsCameraEntity entity, MoveRtsCameraConfig config, DiContainer container)
            : base(entity, config, container) { }
    }
}