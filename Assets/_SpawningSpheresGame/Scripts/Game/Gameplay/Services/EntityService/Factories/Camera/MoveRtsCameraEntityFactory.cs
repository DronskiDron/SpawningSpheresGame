using System;
using MVVMCameraMovement;
using SpawningSpheresGame.Game.Common.DataTypes;
using SpawningSpheresGame.Game.Common.DataTypes.Enums;
using SpawningSpheresGame.Game.Gameplay.Services;
using SpawningSpheresGame.Game.Settings;
using SpawningSpheresGame.Game.Settings.DataStorage;
using SpawningSpheresGame.Game.State;
using SpawningSpheresGame.Game.State.DataTypes;
using SpawningSpheresGame.Game.State.Entities;
using SpawningSpheresGame.Game.State.Entities.Cameras;
using SpawningSpheresGame.Utils.GameplayUtils;
using UnityEngine;
using Zenject;

namespace SpawningSpheresGame.Game.Gameplay.Factories
{
    public class MoveRtsCameraEntityFactory : IEntityFactory
    {
        private readonly DiContainer _container;
        private readonly GameSettings _gameSettings;
        private readonly IGameStateProvider _gameStateProvider;

        public MoveRtsCameraEntityFactory(DiContainer container, GameSettings gameSettings, IGameStateProvider gameStateProvider)
        {
            _container = container;
            _gameSettings = gameSettings;
            _gameStateProvider = gameStateProvider;
        }

        public EntityData CreateEntityData()
        {
            try
            {
                var prefabData = _gameSettings.MainStorage.PrefabsStorage.GetPrefabTotalDataById(PrefabId.RTS_Camera);
                if (prefabData == null)
                {
                    Debug.LogError("RTS_Camera prefab data not found");
                    return null;
                }

                var zoomCameraData = prefabData.GetEntityData<MoveCameraData>(PrefabEntityType.MoveCamera);
                if (zoomCameraData == null)
                {
                    Debug.LogError("MoveCamera specific data not found in CustomData");
                    return null;
                }

                return new MoveRtsCameraEntityData
                {
                    PrefabId = PrefabId.RTS_Camera,
                    TransformStateData = new TransformStateData(
                        zoomCameraData.StartTransform.Position,
                        zoomCameraData.StartTransform.Rotation,
                        zoomCameraData.StartTransform.Scale
                    ),
                    ControlsTransform = true
                };
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error creating MoveCamera entity data: {ex.Message}");
                throw;
            }
        }

        public IEntity<EntityData> CreateEntity(EntityData entityData)
        {
            try
            {
                if (!(entityData is MoveRtsCameraEntityData moveCameraData))
                {
                    throw new ArgumentException($"Invalid entity data type: {entityData.GetType().Name}. Expected MoveRtsCameraEntityData.");
                }

                return new MoveRtsCameraEntity(moveCameraData);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error creating MoveCamera entity: {ex.Message}");
                throw;
            }
        }

        public GameObject CreatePrefab(IEntity<EntityData> entity, Transform container, Vector3? position = null, Quaternion? rotation = null)
        {
            try
            {
                if (!(entity is MoveRtsCameraEntity moveCameraEntity))
                {
                    throw new ArgumentException($"Invalid entity type: {entity.GetType().Name}. Expected MoveRtsCameraEntity.");
                }

                var prefabData = _gameSettings.MainStorage.PrefabsStorage.GetPrefabTotalDataById(moveCameraEntity.PrefabId);
                var pos = position ?? moveCameraEntity.TransformState.Position.Value;
                var rot = rotation ?? moveCameraEntity.TransformState.Rotation.Value;

                return GameObject.Instantiate(prefabData.Prefab, pos, rot, container);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error creating RTS_Camera prefab: {ex.Message}");
                throw;
            }
        }

        public MVVMDataStack CreateMVVMStack(IEntity<EntityData> entity, GameObject prefab, int mapId)
        {
            try
            {
                if (!(entity is MoveRtsCameraEntity moveCameraEntity))
                {
                    throw new ArgumentException($"Invalid entity type: {entity.GetType().Name}. Expected MoveRtsCameraEntity.");
                }

                var moveCameraConfig = _gameSettings.MainStorage.ConfigsStorage.MoveRtsCameraConfig;
                var model = new MoveRtsCameraModel(moveCameraEntity, moveCameraConfig, _container);
                var viewModel = new MoveRtsCameraViewModel(model);
                var view = prefab.GetComponent<MoveRtsCameraBinder>();

                if (view == null)
                {
                    Debug.LogError($"MoveRtsCameraBinder component not found on prefab {prefab.name}");
                    return null;
                }

                view.Init(viewModel);
                return new MVVMDataStack(model, viewModel, view);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error creating MoveCamera MVVM stack: {ex.Message}");
                throw;
            }
        }

        public GameObject RestorePrefab(IEntity<EntityData> entity, Transform container)
        {
            try
            {
                if (!(entity is MoveRtsCameraEntity moveCameraEntity))
                {
                    throw new ArgumentException($"Invalid entity type: {entity.GetType().Name}. Expected MoveRtsCameraEntity.");
                }

                var prefabData = _gameSettings.MainStorage.PrefabsStorage.GetPrefabTotalDataById(moveCameraEntity.PrefabId);
                var pos = moveCameraEntity.TransformState.Position.Value;
                var rot = moveCameraEntity.TransformState.Rotation.Value;

                return GameObject.Instantiate(prefabData.Prefab, pos, rot, container);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error restoring RTS_Camera prefab: {ex.Message}");
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

            GameplayEntitiesId entityId = EntityTypeToGameplayEntityConverter.Convert(entity?.Type ?? EntityType.MoveRtsCameraEntity);
            return prefabManager.CanHostEntityType(entityId);
        }
    }
}