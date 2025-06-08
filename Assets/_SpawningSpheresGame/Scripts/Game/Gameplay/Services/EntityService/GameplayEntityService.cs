using System;
using System.Collections.Generic;
using SpawningSpheresGame.Game.Common.DataTypes;
using SpawningSpheresGame.Game.Common.DataTypes.Enums;
using SpawningSpheresGame.Game.State;
using SpawningSpheresGame.Game.State.Entities;
using UnityEngine;
using Zenject;
using SpawningSpheresGame.Utils.GameplayUtils;
using SpawningSpheresGame.Game.GameRoot.RootManagers;

namespace SpawningSpheresGame.Game.Gameplay.Services
{
    public class GameplayEntityService : IDisposable
    {
        private readonly DiContainer _container;
        private readonly EntityFactoryService _factoryService;
        private readonly EntityStorageService _storageService;
        private readonly EntityStateService _stateService;
        private readonly EntityLifecycleService _lifecycleService;
        private readonly IGameStateProvider _gameStateProvider;
        private readonly PrefabInstanceIdService _simpleRestoreSubservice;
        private readonly TransformControlService _transformControlService;
        private readonly PrefabMatchingService _prefabMatchingService;

        private readonly Dictionary<int, GameObject> _prefabsMap = new();
        private readonly Dictionary<int, MVVMDataStack> _entitiesMap = new();

        private RootSceneContainersManager _rootSceneContainersManager;

        public GameplayEntityService(DiContainer container)
        {
            _container = container;

            _rootSceneContainersManager = container.Resolve<RootSceneContainersManager>();

            _factoryService = new EntityFactoryService(container);
            container.BindInstance(_factoryService);
            _storageService = new EntityStorageService(container, _prefabsMap, _entitiesMap);
            container.BindInstance(_storageService);
            _stateService = new EntityStateService(container);
            _lifecycleService = new EntityLifecycleService(container);
            _gameStateProvider = container.Resolve<IGameStateProvider>();
            _simpleRestoreSubservice = new PrefabInstanceIdService(container);
            container.BindInstance(_simpleRestoreSubservice);
            _transformControlService = new TransformControlService(container);
            _prefabMatchingService = new PrefabMatchingService(container);
            container.BindInstance(_prefabMatchingService);
        }

        public event Action<int, GameObject> OnEntityCreated;
        public event Action<int, GameObject> OnEntityDeleted;

        public int CreateEntity(GameplayEntitiesId entityId, ObjectContainersEnum sceneContainerName
                                , Vector3? position = null, Quaternion? rotation = null)
        {
            return CreateEntityInternal(entityId, sceneContainerName, null, position, rotation);
        }

        public int CreateEntityWithExistingPrefabIfPossible(GameplayEntitiesId entityId, ObjectContainersEnum sceneContainerName,
                        Vector3? position = null, Quaternion? rotation = null)
        {
            GameObject existingPrefab = _prefabMatchingService.FindSuitablePrefab(entityId);
            return CreateEntityInternal(entityId, sceneContainerName, existingPrefab, position, rotation);
        }

        private int CreateEntityInternal(GameplayEntitiesId entityId, ObjectContainersEnum sceneContainerName
                        , GameObject existingPrefab, Vector3? position = null, Quaternion? rotation = null)
        {
            try
            {
                var id = _stateService.GenerateId();
                var factory = _factoryService.GetFactory(entityId);

                var entityData = factory.CreateEntityData();
                entityData.SceneContainerName = sceneContainerName;
                Transform targetContainer = _rootSceneContainersManager.GetWorldContainerByName(sceneContainerName);

                var entity = factory.CreateEntity(entityData);

                _stateService.AssignTempId(entity, id);
                _storageService.RegisterEntity(id, entity);

                GameObject prefab;
                PrefabEntityManager prefabManager;

                if (existingPrefab != null)
                {
                    prefab = existingPrefab;
                    prefabManager = _storageService.GetOrAddPrefabManager(prefab);
                }
                else
                {
                    prefab = factory.CreatePrefab(entity, targetContainer, position, rotation);
                    prefabManager = _storageService.GetOrAddPrefabManager(prefab);

                    int prefabInstanceId = _simpleRestoreSubservice.GetPrefabInstanceId(prefab, prefabManager);
                    prefabManager.SetInstanceId(prefabInstanceId);
                    _simpleRestoreSubservice.AddPrefabInstanceToDictionary(prefabManager.InstanceId);

                }

                entity.PrefabGroupId.Value = prefabManager.PrefabId;
                entity.PrefabInstanceId.Value = prefabManager.InstanceId;

                _stateService.AddEntityToGameState(entity);
                _storageService.RegisterPrefab(id, prefab, entityId, entity.PrefabInstanceId.Value);

                var mvvmStack = factory.CreateMVVMStack(entity, prefab, id);

                if (mvvmStack == null)
                {
                    RollbackEntityCreation(id, entity);
                    return -1;
                }

                _storageService.RegisterMVVMStack(id, mvvmStack);
                RegisterBinderInPrefabManager(prefab, mvvmStack.View, mvvmStack.View.GetType(), id);

                _lifecycleService.RegisterTickables(mvvmStack);

                _transformControlService.SubscribeEntityToGoTransform(prefab.transform, entity);

                _gameStateProvider.SaveGameState();
                OnEntityCreated?.Invoke(id, prefab);
                return id;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Ошибка создания сущности {entityId}: {ex.Message}\n{ex.StackTrace}");
                return -1;
            }
        }

        private void RollbackEntityCreation(int id, IEntity<EntityData> entity)
        {
            _stateService.RemoveEntityFromGameState(id);
            _storageService.UnregisterPrefab(id);
            _storageService.UnregisterEntity(id);
        }

        public void DeleteEntity(int mapId)
        {
            try
            {
                var entity = _storageService.GetEntity(mapId);
                if (entity == null) return;

                var mvvmStack = _storageService.GetMVVMStack(mapId);
                if (mvvmStack == null) return;

                var prefab = _storageService.GetPrefab(mapId);
                Debug.Log("DeleteEntity!!!");

                bool canDestroyPrefab = prefab != null && _storageService.CanDestroyPrefab(mapId);
                int entityTempId = entity.TempId.Value;

                _lifecycleService.UnregisterTickables(mvvmStack);

                if (prefab != null)
                {
                    UnregisterBinderFromPrefabManager(prefab, mvvmStack.View, mvvmStack.View.GetType(), mapId);
                }

                _lifecycleService.DestroyMVVMStack(mvvmStack);

                _transformControlService.UnsubscribeEntityFromGoTransform(mapId);
                _simpleRestoreSubservice.RemoveEntityFromDictionary(mapId, entity.PrefabInstanceId.Value);
                _stateService.RemoveEntityFromGameState(mapId);

                _storageService.UnregisterMVVMStack(mapId);
                _storageService.UnregisterEntity(mapId);
                _storageService.UnregisterPrefab(mapId);

                if (canDestroyPrefab)
                {
                    GameObject.Destroy(prefab);
                }

                _gameStateProvider.SaveGameState();
                OnEntityDeleted?.Invoke(mapId, prefab);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Ошибка удаления сущности {mapId}: {ex.Message}\n{ex.StackTrace}");
            }
        }

        private void RegisterBinderInPrefabManager(GameObject prefab, IView binder, Type binderType, int entityId)
        {
            var prefabManager = _storageService.GetOrAddPrefabManager(prefab);
            prefabManager.RegisterBinder(binder, binderType, entityId);
        }

        private void UnregisterBinderFromPrefabManager(GameObject prefab, IView binder, Type binderType, int entityId)
        {
            var prefabManager = prefab.GetComponent<PrefabEntityManager>();
            if (prefabManager != null)
            {
                prefabManager.UnregisterBinder(binder, binderType, entityId);
            }
        }

        public int RestoreEntity<T>(T entity) where T : IEntity<EntityData>
        {
            try
            {
                var id = _stateService.GenerateId();
                var entityType = EntityTypeToGameplayEntityConverter.Convert(entity.Type);
                var factory = _factoryService.GetFactory(entityType);

                _stateService.AssignTempId(entity, id);
                _storageService.RegisterEntity(id, entity);

                Transform targetContainer = _rootSceneContainersManager.GetWorldContainerByName(entity.SceneContainerName.Value);
                var prefab = factory.RestorePrefab(entity, targetContainer);

                _simpleRestoreSubservice.AddPrefabInstanceToDictionary(entity.PrefabInstanceId.Value);
                _simpleRestoreSubservice.AddEntityToDictionary(id, entity.PrefabInstanceId.Value);

                _storageService.RegisterPrefab(id, prefab, entityType, entity.PrefabInstanceId.Value);

                var mvvmStack = factory.RestoreMVVMStack(entity, prefab, id);
                _storageService.RegisterMVVMStack(id, mvvmStack);

                RegisterBinderInPrefabManager(prefab, mvvmStack.View, mvvmStack.View.GetType(), id);

                var prefabManager = prefab.GetComponent<PrefabEntityManager>();
                prefabManager.SetInstanceId(entity.PrefabInstanceId.Value);

                _lifecycleService.RegisterTickables(mvvmStack);

                _transformControlService.SubscribeEntityToGoTransform(prefab.transform, entity);

                OnEntityCreated?.Invoke(id, prefab);
                return id;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Ошибка восстановления сущности: {ex.Message}\n{ex.StackTrace}");
                return -1;
            }
        }

        public int RestoreEntityOnPrefab(IEntity<EntityData> entity, GameObject existingPrefab)
        {
            try
            {
                var id = _stateService.GenerateId();
                var entityType = EntityTypeToGameplayEntityConverter.Convert(entity.Type);
                var factory = _factoryService.GetFactory(entityType);

                _stateService.AssignTempId(entity, id);
                _simpleRestoreSubservice.AddEntityToDictionary(id, entity.PrefabInstanceId.Value);
                _storageService.RegisterEntity(id, entity);

                var prefabManager = _storageService.GetOrAddPrefabManager(existingPrefab);
                entity.PrefabGroupId.Value = prefabManager.PrefabId;

                _storageService.RegisterPrefab(id, existingPrefab, entityType, entity.PrefabInstanceId.Value);

                var mvvmStack = factory.CreateMVVMStack(entity, existingPrefab, id);
                if (mvvmStack == null)
                {
                    _storageService.UnregisterPrefab(id);
                    _storageService.UnregisterEntity(id);
                    return -1;
                }

                _storageService.RegisterMVVMStack(id, mvvmStack);
                RegisterBinderInPrefabManager(existingPrefab, mvvmStack.View, mvvmStack.View.GetType(), id);

                _lifecycleService.RegisterTickables(mvvmStack);

                _transformControlService.SubscribeEntityToGoTransform(existingPrefab.transform, entity);

                OnEntityCreated?.Invoke(id, existingPrefab);

                return id;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Ошибка восстановления сущности на префабе: {ex.Message}\n{ex.StackTrace}");
                return -1;
            }
        }

        private bool _isDisposed = false;

        public void Dispose()
        {
            if (_isDisposed) return;
            _isDisposed = true;

            OnEntityCreated = null;
            OnEntityDeleted = null;
            _rootSceneContainersManager = null;

            _prefabMatchingService?.Dispose();
            _transformControlService?.Dispose();
            _simpleRestoreSubservice?.Dispose();
            _lifecycleService?.Dispose();
            _stateService?.Dispose();
            _storageService?.Dispose();
            _factoryService?.Dispose();

            _prefabsMap?.Clear();
            _entitiesMap?.Clear();
        }
    }
}