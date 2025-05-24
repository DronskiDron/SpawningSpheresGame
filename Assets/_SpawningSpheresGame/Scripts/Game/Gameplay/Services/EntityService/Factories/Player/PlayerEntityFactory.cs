using System;
using SpawningSpheresGame.Game.Settings;
using SpawningSpheresGame.Game.Settings.DataStorage;
using SpawningSpheresGame.Game.State;
using SpawningSpheresGame.Game.State.Entities;
using SpawningSpheresGame.Game.State.Entities.Creatures.Player;
using SpawningSpheresGame.Game.State.DataTypes;
using PlayerController.MVVM.Player;
using UnityEngine;
using Zenject;
using SpawningSpheresGame.Game.Common.DataTypes;
using SpawningSpheresGame.Game.Gameplay.Services;
using SpawningSpheresGame.Game.Common.DataTypes.Enums;
using SpawningSpheresGame.Utils.GameplayUtils;

namespace SpawningSpheresGame.Game.Gameplay.Factories
{
    public class PlayerEntityFactory : IEntityFactory
    {
        private readonly DiContainer _container;
        private readonly GameSettings _gameSettings;
        private readonly IGameStateProvider _gameStateProvider;

        public PlayerEntityFactory(DiContainer container, GameSettings gameSettings, IGameStateProvider gameStateProvider)
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

                return new PlayerEntityData
                {
                    PrefabId = PrefabId.Player,
                    TransformStateData = new TransformStateData(
                        playerData.StartTransform.Position,
                        playerData.StartTransform.Rotation,
                        playerData.StartTransform.Scale
                    ),
                    PlayerCameraOffsetData = new TransformStateData(
                        playerData.StartCameraOffset.Position,
                        playerData.StartCameraOffset.Rotation,
                        playerData.StartCameraOffset.Scale
                    ),
                    ControlsTransform = true
                };
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error creating player entity data: {ex.Message}");
                throw;
            }
        }

        public IEntity<EntityData> CreateEntity(EntityData entityData)
        {
            try
            {
                if (!(entityData is PlayerEntityData playerData))
                {
                    throw new ArgumentException($"Invalid entity data type: {entityData.GetType().Name}. Expected PlayerEntityData.");
                }

                return new PlayerEntity(playerData);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error creating player entity: {ex.Message}");
                throw;
            }
        }

        public GameObject CreatePrefab(IEntity<EntityData> entity, Transform container, Vector3? position = null, Quaternion? rotation = null)
        {
            try
            {
                if (!(entity is PlayerEntity playerEntity))
                {
                    throw new ArgumentException($"Invalid entity type: {entity.GetType().Name}. Expected PlayerEntity.");
                }

                var prefabData = _gameSettings.MainStorage.PrefabsStorage.GetPrefabTotalDataById(playerEntity.PrefabId);
                var pos = position ?? playerEntity.TransformState.Position.Value;
                var rot = rotation ?? playerEntity.TransformState.Rotation.Value;

                return GameObject.Instantiate(prefabData.Prefab, pos, rot, container);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error creating player prefab: {ex.Message}");
                throw;
            }
        }

        public MVVMDataStack CreateMVVMStack(IEntity<EntityData> entity, GameObject prefab, int mapId)
        {
            try
            {
                if (!(entity is PlayerEntity playerEntity))
                {
                    throw new ArgumentException($"Invalid entity type: {entity.GetType().Name}. Expected PlayerEntity.");
                }

                var playerConfig = _gameSettings.MainStorage.ConfigsStorage.PlayerConfig;
                var model = new PlayerModel(playerEntity, playerConfig, _container);
                var viewModel = new PlayerViewModel(model);
                var view = prefab.GetComponent<PlayerBinder>();

                if (view == null)
                {
                    Debug.LogError($"PlayerBinder component not found on prefab {prefab.name}");
                    return null;
                }

                view.Init(viewModel);
                return new MVVMDataStack(model, viewModel, view);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error creating player MVVM stack: {ex.Message}");
                throw;
            }
        }

        public GameObject RestorePrefab(IEntity<EntityData> entity, Transform container)
        {
            try
            {
                if (!(entity is PlayerEntity playerEntity))
                {
                    throw new ArgumentException($"Invalid entity type: {entity.GetType().Name}. Expected PlayerEntity.");
                }

                var prefabData = _gameSettings.MainStorage.PrefabsStorage.GetPrefabTotalDataById(playerEntity.PrefabId);
                var pos = playerEntity.TransformState.Position.Value;
                var rot = playerEntity.TransformState.Rotation.Value;

                return GameObject.Instantiate(prefabData.Prefab, pos, rot, container);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error restoring player prefab: {ex.Message}");
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

            GameplayEntitiesId entityId = EntityTypeToGameplayEntityConverter.Convert(entity?.Type ?? EntityType.PlayerEntity);
            return prefabManager.CanHostEntityType(entityId);
        }
    }
}