using System;
using MVVMZoomCamera;
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
    public class ZoomRtsCameraEntityFactory : IEntityFactory
    {
        private readonly DiContainer _container;
        private readonly GameSettings _gameSettings;
        private readonly IGameStateProvider _gameStateProvider;

        public ZoomRtsCameraEntityFactory(DiContainer container, GameSettings gameSettings, IGameStateProvider gameStateProvider)
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

                var zoomCameraData = prefabData.GetEntityData<ZoomCameraData>(PrefabEntityType.ZoomCamera);
                if (zoomCameraData == null)
                {
                    Debug.LogError("ZoomCamera specific data not found in CustomData");
                    return null;
                }

                return new ZoomRtsCameraEntityData
                {
                    PrefabId = PrefabId.RTS_Camera,
                    TransformStateData = new TransformStateData(
                        zoomCameraData.StartTransform.Position,
                        zoomCameraData.StartTransform.Rotation,
                        zoomCameraData.StartTransform.Scale
                    ),
                    FieldOfView = zoomCameraData.StartZoomValue.FieldOfView,
                    OrthographicSize = zoomCameraData.StartZoomValue.OrthographicSize,
                    ControlsTransform = false
                };
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error creating ZoomCamera entity data: {ex.Message}");
                throw;
            }
        }

        public IEntity<EntityData> CreateEntity(EntityData entityData)
        {
            try
            {
                if (!(entityData is ZoomRtsCameraEntityData zoomCameraData))
                {
                    throw new ArgumentException($"Invalid entity data type: {entityData.GetType().Name}. Expected ZoomRtsCameraEntityData.");
                }

                return new ZoomRtsCameraEntity(zoomCameraData);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error creating ZoomCamera entity: {ex.Message}");
                throw;
            }
        }

        public GameObject CreatePrefab(IEntity<EntityData> entity, Transform container, Vector3? position = null, Quaternion? rotation = null)
        {
            try
            {
                if (!(entity is ZoomRtsCameraEntity zoomCameraEntity))
                {
                    throw new ArgumentException($"Invalid entity type: {entity.GetType().Name}. Expected ZoomRtsCameraEntity.");
                }

                var prefabData = _gameSettings.MainStorage.PrefabsStorage.GetPrefabTotalDataById(zoomCameraEntity.PrefabId);
                var pos = position ?? zoomCameraEntity.TransformState.Position.Value;
                var rot = rotation ?? zoomCameraEntity.TransformState.Rotation.Value;

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
                if (!(entity is ZoomRtsCameraEntity zoomCameraEntity))
                {
                    throw new ArgumentException($"Invalid entity type: {entity.GetType().Name}. Expected ZoomRtsCameraEntity.");
                }

                var zoomCameraConfig = _gameSettings.MainStorage.ConfigsStorage.ZoomRtsCameraConfig;
                var model = new ZoomRtsCameraModel(zoomCameraEntity, zoomCameraConfig, _container);
                var viewModel = new ZoomRtsCameraViewModel(model);
                var view = prefab.GetComponent<ZoomRtsCameraBinder>();

                if (view == null)
                {
                    Debug.LogError($"ZoomRtsCameraBinder component not found on prefab {prefab.name}");
                    return null;
                }

                view.Init(viewModel);
                return new MVVMDataStack(model, viewModel, view);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error creating ZoomCamera MVVM stack: {ex.Message}");
                throw;
            }
        }

        public GameObject RestorePrefab(IEntity<EntityData> entity, Transform container)
        {
            try
            {
                if (!(entity is ZoomRtsCameraEntity zoomCameraEntity))
                {
                    throw new ArgumentException($"Invalid entity type: {entity.GetType().Name}. Expected ZoomRtsCameraEntity.");
                }

                var prefabData = _gameSettings.MainStorage.PrefabsStorage.GetPrefabTotalDataById(zoomCameraEntity.PrefabId);
                var pos = zoomCameraEntity.TransformState.Position.Value;
                var rot = zoomCameraEntity.TransformState.Rotation.Value;

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

            GameplayEntitiesId entityId = EntityTypeToGameplayEntityConverter.Convert(entity?.Type ?? EntityType.ZoomRtsCameraEntity);
            return prefabManager.CanHostEntityType(entityId);
        }
    }
}