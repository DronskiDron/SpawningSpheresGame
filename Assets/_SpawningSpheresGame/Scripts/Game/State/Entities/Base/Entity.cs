using R3;
using SpawningSpheresGame.Game.GameRoot.RootManagers;
using SpawningSpheresGame.Game.Settings.DataStorage;
using SpawningSpheresGame.Game.State.DataTypes;

namespace SpawningSpheresGame.Game.State.Entities
{
    public abstract class Entity<TData> : IEntity<TData> where TData : EntityData
    {
        public TData Origin { get; }
        public int UniqueId => Origin.UniqueId;
        public EntityType Type => Origin.Type;
        public PrefabId PrefabId { get; }
        public TransformState TransformState { get; }
        public ReactiveProperty<int> TempId { get; }
        public ReactiveProperty<string> PrefabGroupId { get; }
        public ReactiveProperty<int> PrefabInstanceId { get; }
        public ReactiveProperty<bool> ControlsTransform { get; }
        public ReactiveProperty<ObjectContainersEnum> SceneContainerName { get; }

        protected Entity(TData data)
        {
            Origin = data;
            PrefabId = data.PrefabId;
            TransformState = new TransformState(data.TransformStateData);

            TempId = new ReactiveProperty<int>(data.TempId);
            TempId.Skip(1).Subscribe(value => data.TempId = value);

            PrefabGroupId = new ReactiveProperty<string>(data.PrefabGroupId ?? string.Empty);
            PrefabGroupId.Skip(1).Subscribe(value => data.PrefabGroupId = value);

            PrefabInstanceId = new ReactiveProperty<int>(data.PrefabInstanceId);
            PrefabInstanceId.Skip(1).Subscribe(value => data.PrefabInstanceId = value);

            ControlsTransform = new ReactiveProperty<bool>(data.ControlsTransform);
            ControlsTransform.Skip(1).Subscribe(value => data.ControlsTransform = value);

            SceneContainerName = new ReactiveProperty<ObjectContainersEnum>(data.SceneContainerName);
            SceneContainerName.Skip(1).Subscribe(value => data.SceneContainerName = value);
        }

        public virtual void Dispose()
        {
            TransformState.Dispose();
        }
    }

    public class Entity : Entity<EntityData>, IEntity
    {
        public Entity(EntityData data) : base(data) { }
    }
}