using System;
using SpawningSpheresGame.Game.GameRoot.RootManagers;
using SpawningSpheresGame.Game.Settings.DataStorage;
using SpawningSpheresGame.Game.State.DataTypes;

namespace SpawningSpheresGame.Game.State.Entities
{
    [Serializable]
    public abstract class EntityData
    {
        public int UniqueId { get; set; }
        public int TempId { get; set; }
        public EntityType Type { get; set; }
        public PrefabId PrefabId { get; set; }
        public string PrefabGroupId { get; set; }
        public int PrefabInstanceId { get; set; }
        public bool ControlsTransform { get; set; } = false;
        public TransformStateData TransformStateData = new();
        public ObjectContainersEnum SceneContainerName { get; set; }

        protected EntityData()
        {
            Type = EntityType.BaseEntity;
            PrefabGroupId = string.Empty;
        }
    }
}