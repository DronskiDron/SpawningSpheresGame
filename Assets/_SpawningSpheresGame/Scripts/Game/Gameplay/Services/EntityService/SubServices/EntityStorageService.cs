using System;
using System.Collections.Generic;
using SpawningSpheresGame.Game.Common.DataTypes;
using SpawningSpheresGame.Game.Common.DataTypes.Enums;
using SpawningSpheresGame.Game.State.Entities;
using UnityEngine;
using Zenject;

namespace SpawningSpheresGame.Game.Gameplay.Services
{
    public class EntityStorageService : IDisposable
    {
        private readonly Dictionary<int, GameObject> _prefabsMap;
        private readonly Dictionary<int, MVVMDataStack> _entitiesMap;
        private readonly Dictionary<string, List<int>> _prefabIdToEntityIds = new();
        private readonly Dictionary<int, GameObject> _prefabInstanceIdMap = new();
        private readonly Dictionary<GameplayEntitiesId, List<string>> _entityTypeToActivePreafabIds = new();
        private readonly Dictionary<int, IEntity<EntityData>> _rawEntitiesMap = new();

        public EntityStorageService(DiContainer container,
                                    Dictionary<int, GameObject> prefabsMap,
                                    Dictionary<int, MVVMDataStack> entitiesMap)
        {
            _prefabsMap = prefabsMap;
            _entitiesMap = entitiesMap;
        }

        public void RegisterPrefab(int mapId, GameObject prefab, GameplayEntitiesId entityType, int prefabInstanceId)
        {
            _prefabsMap[mapId] = prefab;
            RegisterPrefabInstanceId(prefabInstanceId, prefab);

            var prefabManager = GetOrAddPrefabManager(prefab);
            string prefabId = prefabManager.PrefabId;

            RegisterPrefabEntityMapping(mapId, prefabId, entityType);
        }

        private void RegisterPrefabInstanceId(int prefabInstanceId, GameObject prefab)
        {
            if (!_prefabInstanceIdMap.ContainsKey(prefabInstanceId))
                _prefabInstanceIdMap[prefabInstanceId] = prefab;
        }

        public void UnRegisterPrefabInstanceId(int prefabInstanceId)
        {
            if (_prefabInstanceIdMap.ContainsKey(prefabInstanceId))
                _prefabInstanceIdMap.Remove(prefabInstanceId);
        }

        private void RegisterPrefabEntityMapping(int mapId, string prefabId, GameplayEntitiesId entityType)
        {
            if (!_prefabIdToEntityIds.ContainsKey(prefabId))
            {
                _prefabIdToEntityIds[prefabId] = new List<int>();
            }
            _prefabIdToEntityIds[prefabId].Add(mapId);

            if (!_entityTypeToActivePreafabIds.ContainsKey(entityType))
            {
                _entityTypeToActivePreafabIds[entityType] = new List<string>();
            }

            if (!_entityTypeToActivePreafabIds[entityType].Contains(prefabId))
            {
                _entityTypeToActivePreafabIds[entityType].Add(prefabId);
            }
        }

        public void UnregisterPrefab(int mapId)
        {
            var prefab = GetPrefab(mapId);
            Debug.Log("UnregisterPrefab!!!");
            if (prefab == null) return;

            var prefabManager = prefab.GetComponent<PrefabEntityManager>();
            if (prefabManager == null)
            {
                _prefabsMap.Remove(mapId);
                return;
            }

            string prefabId = prefabManager.PrefabId;
            UnregisterPrefabEntityMapping(mapId, prefabId);
        }

        private void UnregisterPrefabEntityMapping(int mapId, string prefabId)
        {
            if (!_prefabIdToEntityIds.ContainsKey(prefabId)) return;

            _prefabIdToEntityIds[prefabId].Remove(mapId);

            if (_prefabIdToEntityIds[prefabId].Count == 0)
            {
                _prefabIdToEntityIds.Remove(prefabId);

                foreach (var kvp in _entityTypeToActivePreafabIds)
                {
                    kvp.Value.Remove(prefabId);
                }

                _prefabsMap.Remove(mapId);
            }
        }

        public GameObject GetPrefab(int mapId)
        {
            _prefabsMap.TryGetValue(mapId, out var prefab);
            if (prefab != null)
            {
                return prefab;
            }

            Debug.LogError($"Prefab with mapId {mapId} is null!");
            return prefab;
        }

        public GameObject GetPrefabByPrefabInstanceId(int prefabInstanceId)
        {
            if (_prefabInstanceIdMap.TryGetValue(prefabInstanceId, out var go))
                return go;

            Debug.LogError($"Prefab with prefabInstanceId {prefabInstanceId} is null!");
            return go;
        }

        public List<GameObject> GetPrefabsOfType(GameplayEntitiesId entityType)
        {
            List<GameObject> result = new List<GameObject>();

            if (_entityTypeToActivePreafabIds.TryGetValue(entityType, out var prefabIds))
            {
                foreach (var prefabId in prefabIds)
                {
                    if (_prefabIdToEntityIds.TryGetValue(prefabId, out var entityIds) && entityIds.Count > 0)
                    {
                        var entityId = entityIds[0];
                        if (_prefabsMap.TryGetValue(entityId, out var prefab))
                        {
                            result.Add(prefab);
                        }
                    }
                }
            }

            return result;
        }

        public void RegisterMVVMStack(int mapId, MVVMDataStack mvvmStack)
        {
            if (mvvmStack == null)
            {
                Debug.LogError($"Попытка зарегистрировать null MVVMStack для mapId {mapId}");
                return;
            }

            ValidateMVVMStack(mvvmStack, mapId);

            _entitiesMap[mapId] = mvvmStack;
        }

        private void ValidateMVVMStack(MVVMDataStack mvvmStack, int mapId)
        {
            if (mvvmStack.Model == null)
            {
                Debug.LogError($"MVVMStack для mapId {mapId} имеет null Model!");
            }

            if (mvvmStack.View == null)
            {
                Debug.LogError($"MVVMStack для mapId {mapId} имеет null View!");
            }
        }

        public void UnregisterMVVMStack(int mapId)
        {
            _entitiesMap.Remove(mapId);
        }

        public MVVMDataStack GetMVVMStack(int mapId)
        {
            _entitiesMap.TryGetValue(mapId, out var stack);
            return stack;
        }

        public PrefabEntityManager GetOrAddPrefabManager(GameObject prefab)
        {
            var prefabManager = prefab.GetComponent<PrefabEntityManager>();
            if (prefabManager == null)
            {
                prefabManager = prefab.AddComponent<PrefabEntityManager>();
            }
            return prefabManager;
        }

        public bool CanDestroyPrefab(int mapId)
        {
            var prefab = GetPrefab(mapId);

            if (prefab == null) return true;

            var prefabManager = prefab.GetComponent<PrefabEntityManager>();
            if (prefabManager == null) return true;

            string prefabId = prefabManager.PrefabId;

            if (_prefabIdToEntityIds.TryGetValue(prefabId, out var entityIds))
            {
                return entityIds.Count <= 1 && entityIds.Contains(mapId);
            }

            return true;
        }

        public bool HasBinderOfType(GameObject prefab, Type binderType)
        {
            var prefabManager = prefab.GetComponent<PrefabEntityManager>();
            return prefabManager != null && prefabManager.HasBinderOfType(binderType);
        }

        public void RegisterEntity(int mapId, IEntity<EntityData> entity)
        {
            _rawEntitiesMap[mapId] = entity;
        }

        public IEntity<EntityData> GetEntity(int mapId)
        {
            _rawEntitiesMap.TryGetValue(mapId, out var entity);
            return entity;
        }

        public void UnregisterEntity(int mapId)
        {
            _rawEntitiesMap.Remove(mapId);
        }

        public List<GameObject> GetAllPrefabs()
        {
            List<GameObject> result = new List<GameObject>();

            foreach (var entityType in Enum.GetValues(typeof(GameplayEntitiesId)))
            {
                var prefabsOfType = GetPrefabsOfType((GameplayEntitiesId)entityType);
                foreach (var prefab in prefabsOfType)
                {
                    if (!result.Contains(prefab))
                    {
                        result.Add(prefab);
                    }
                }
            }

            return result;
        }

        private bool _isDisposed = false;

        public void Dispose()
        {
            if (_isDisposed) return;
            _isDisposed = true;

            _prefabIdToEntityIds.Clear();
            _prefabInstanceIdMap.Clear();
            _entityTypeToActivePreafabIds.Clear();
            _rawEntitiesMap.Clear();
        }
    }
}