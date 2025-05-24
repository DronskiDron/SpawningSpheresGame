using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace SpawningSpheresGame.Game.Gameplay.Services
{
    public class PrefabInstanceIdService : IDisposable
    {
        private readonly EntityStorageService _entityStorageService;
        private readonly Dictionary<int, List<int>> _prefabInstanceDictionary = new();

        public PrefabInstanceIdService(DiContainer container)
        {
            _entityStorageService = container.Resolve<EntityStorageService>();
        }

        public int GetPrefabInstanceId(GameObject prefab, PrefabEntityManager prefabManager)
        {
            if (prefabManager != null && prefabManager.InstanceId > 0)
            {
                return prefabManager.InstanceId;
            }

            int id = Math.Abs(prefab.GetInstanceID());
            if (prefabManager != null)
            {
                prefabManager.SetInstanceId(id);
            }
            return id;
        }

        public void AddPrefabInstanceToDictionary(int prefabInstanceId)
        {
            _prefabInstanceDictionary[prefabInstanceId] = new List<int>();
        }

        public void RemovePrefabInstanceFromDictionary(int prefabInstanceId)
        {
            if (_prefabInstanceDictionary.ContainsKey(prefabInstanceId))
            {
                _prefabInstanceDictionary.Remove(prefabInstanceId);
                _entityStorageService.UnRegisterPrefabInstanceId(prefabInstanceId);
            }
        }

        public void AddEntityToDictionary(int entityTempId, int prefabInstanceId)
        {
            if (_prefabInstanceDictionary.TryGetValue(prefabInstanceId, out var list))
            {
                list.Add(entityTempId);
            }
        }

        public void RemoveEntityFromDictionary(int entityTempId, int prefabInstanceId)
        {
            if (_prefabInstanceDictionary.TryGetValue(prefabInstanceId, out var list))
            {
                list.Remove(entityTempId);

                if (list.Count == 0)
                {
                    _prefabInstanceDictionary.Remove(prefabInstanceId);
                    _entityStorageService.UnRegisterPrefabInstanceId(prefabInstanceId);
                }
            }
        }

        public bool CheckPrefabInstanceIdIsAlreadyExists(int prefabInstanceId)
        {
            var result = _prefabInstanceDictionary.ContainsKey(prefabInstanceId);

            return result;
        }

        public GameObject GetAlreadyExistingPrefabByPrefabInstanceId(int prefabInstanceId)
        {
            var go = _entityStorageService.GetPrefabByPrefabInstanceId(prefabInstanceId);

            return go;
        }

        private bool _isDisposed = false;

        public void Dispose()
        {
            if (_isDisposed) return;
            _isDisposed = true;

            _prefabInstanceDictionary.Clear();
        }
    }
}