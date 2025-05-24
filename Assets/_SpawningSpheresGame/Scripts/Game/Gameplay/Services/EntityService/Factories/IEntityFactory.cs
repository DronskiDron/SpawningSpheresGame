using SpawningSpheresGame.Game.Common.DataTypes;
using SpawningSpheresGame.Game.State.Entities;
using UnityEngine;

namespace SpawningSpheresGame.Game.Gameplay.Factories
{
    public interface IEntityFactory
    {
        EntityData CreateEntityData();
        IEntity<EntityData> CreateEntity(EntityData entityData);
        GameObject CreatePrefab(IEntity<EntityData> entity, Transform container, Vector3? position = null, Quaternion? rotation = null);
        MVVMDataStack CreateMVVMStack(IEntity<EntityData> entity, GameObject prefab, int mapId);
        GameObject RestorePrefab(IEntity<EntityData> entity, Transform container);
        MVVMDataStack RestoreMVVMStack(IEntity<EntityData> entity, GameObject prefab, int mapId);
        bool CanUseExistingPrefab(GameObject existingPrefab, IEntity<EntityData> entity);
    }
}