using System;
using Gameplay.MVVMGroups.Test;
using SpawningSpheresGame.Game.Common.DataTypes;
using SpawningSpheresGame.Game.Common.DataTypes.Enums;
using SpawningSpheresGame.Game.Gameplay.Services;
using SpawningSpheresGame.Game.Settings;
using SpawningSpheresGame.Game.Settings.DataStorage;
using SpawningSpheresGame.Game.State;
using SpawningSpheresGame.Game.State.DataTypes;
using SpawningSpheresGame.Game.State.Entities;
using SpawningSpheresGame.Game.State.Entities.Test;
using SpawningSpheresGame.Utils.GameplayUtils;
using UnityEngine;
using Zenject;

namespace SpawningSpheresGame.Game.Gameplay.Factories
{
    public class TestEntityFactory : IEntityFactory
    {
        private readonly DiContainer _container;
        private readonly GameSettings _gameSettings;
        private readonly IGameStateProvider _gameStateProvider;

        public TestEntityFactory(
            DiContainer container,
            GameSettings gameSettings,
            IGameStateProvider gameStateProvider)
        {
            _container = container;
            _gameSettings = gameSettings;
            _gameStateProvider = gameStateProvider;
        }

        public EntityData CreateEntityData()
        {
            try
            {
                var prefabData = _gameSettings.MainStorage.PrefabsStorage.GetPrefabTotalDataById(PrefabId.Player);
                if (prefabData == null)
                {
                    Debug.LogError("Player prefab data not found");
                    return null;
                }

                var playerData = prefabData.GetEntityData<PlayerData>(PrefabEntityType.Player);
                if (playerData == null)
                {
                    Debug.LogError("Player specific data not found in CustomData");
                    return null;
                }

                return new TestEntityData
                {
                    PrefabId = PrefabId.Player,
                    Message = $"Test Entity #{Time.time:F2}",
                    TransformStateData = new TransformStateData(
                            playerData.StartTransform.Position,
                            playerData.StartTransform.Rotation,
                            playerData.StartTransform.Scale
                    ),
                    ControlsTransform = false
                };
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error creating test entity data: {ex.Message}");
                throw;
            }
        }

        public IEntity<EntityData> CreateEntity(EntityData entityData)
        {
            try
            {
                if (!(entityData is TestEntityData testEntityData))
                {
                    throw new ArgumentException($"Invalid entity data type: {entityData.GetType().Name}. Expected TestEntityData.");
                }

                return new TestEntity(testEntityData);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error creating test entity: {ex.Message}");
                throw;
            }
        }

        public GameObject CreatePrefab(IEntity<EntityData> entity, Transform container, Vector3? position = null, Quaternion? rotation = null)
        {
            try
            {
                if (!(entity is TestEntity testEntity))
                {
                    throw new ArgumentException($"Invalid entity type: {entity.GetType().Name}. Expected TestEntity.");
                }

                var prefabData = _gameSettings.MainStorage.PrefabsStorage.GetPrefabTotalDataById(PrefabId.Player);
                var playerData = prefabData.GetEntityData<PlayerData>(PrefabEntityType.Player);
                var pos = position ?? playerData.StartTransform.Position;
                var rot = rotation ?? playerData.StartTransform.Rotation;

                return GameObject.Instantiate(prefabData.Prefab, pos, rot, container);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error creating test prefab: {ex.Message}");
                throw;
            }
        }

        public MVVMDataStack CreateMVVMStack(IEntity<EntityData> entity, GameObject prefab, int mapId)
        {
            try
            {
                if (!(entity is TestEntity testEntity))
                {
                    throw new ArgumentException($"Invalid entity type: {entity.GetType().Name}. Expected TestEntity.");
                }

                var model = new TestModel(testEntity, _gameSettings.MainStorage.ConfigsStorage.TestConfig, _container);
                var viewModel = new TestViewModel(model);
                _container.Inject(viewModel);

                var binder = prefab.GetComponent<TestBinder>();
                if (binder == null)
                {
                    Debug.LogError($"TestBinder component not found on prefab {prefab.name}");
                    return null;
                }

                binder.Init(viewModel);
                return new MVVMDataStack(model, viewModel, binder);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error creating test MVVM stack: {ex.Message}");
                return null;
            }
        }

        public GameObject RestorePrefab(IEntity<EntityData> entity, Transform container)
        {
            try
            {
                if (!(entity is TestEntity testEntity))
                {
                    throw new ArgumentException($"Invalid entity type: {entity.GetType().Name}. Expected TestEntity.");
                }

                var prefabData = _gameSettings.MainStorage.PrefabsStorage.GetPrefabTotalDataById(testEntity.PrefabId);
                var pos = testEntity.TransformState.Position.Value;
                var rot = testEntity.TransformState.Rotation.Value;

                return GameObject.Instantiate(prefabData.Prefab, pos, rot, container);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error restoring test prefab: {ex.Message}");
                throw;
            }
        }

        public MVVMDataStack RestoreMVVMStack(IEntity<EntityData> entity, GameObject prefab, int mapId)
        {
            return CreateMVVMStack(entity, prefab, mapId);
        }

        public bool CanUseExistingPrefab(GameObject existingPrefab, IEntity<EntityData> entity)
        {
            var prefabManager = existingPrefab.GetComponent<PrefabEntityManager>();
            if (prefabManager == null) return false;

            GameplayEntitiesId entityId = EntityTypeToGameplayEntityConverter.Convert(entity?.Type ?? EntityType.TestEntity);
            return prefabManager.CanHostEntityType(entityId);
        }
    }
}