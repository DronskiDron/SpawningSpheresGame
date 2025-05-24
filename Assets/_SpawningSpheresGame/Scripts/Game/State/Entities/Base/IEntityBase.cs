using System;
using R3;
using SpawningSpheresGame.Game.GameRoot.RootManagers;
using SpawningSpheresGame.Game.Settings.DataStorage;
using SpawningSpheresGame.Game.State.DataTypes;

namespace SpawningSpheresGame.Game.State.Entities
{
    public interface IEntityBase : IDisposable
    {
        int UniqueId { get; }
        EntityType Type { get; }
        PrefabId PrefabId { get; }
        TransformState TransformState { get; }
        ReactiveProperty<int> TempId { get; }
        ReactiveProperty<string> PrefabGroupId { get; }
        ReactiveProperty<int> PrefabInstanceId { get; }
        ReactiveProperty<bool> ControlsTransform { get; }
        ReactiveProperty<ObjectContainersEnum> SceneContainerName { get; }
    }
}