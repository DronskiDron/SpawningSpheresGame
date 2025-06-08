using System;
using SpawningSpheresGame.Game.Common.DataTypes.Enums;
using SpawningSpheresGame.Utils.GameplayUtils;
using UnityEngine;
using Zenject;

namespace SpawningSpheresGame.Game.Gameplay.Services
{
    public class PrefabMatchingService : IDisposable
    {
        private readonly EntityStorageService _storageService;

        public PrefabMatchingService(DiContainer container)
        {
            _storageService = container.Resolve<EntityStorageService>();
        }

        public GameObject FindSuitablePrefab(GameplayEntitiesId entityId)
        {
            Type binderType = EntityBinderTypeRegistry.GetBinderType(entityId);
            if (binderType == null) return null;

            var prefabsOfSameType = _storageService.GetPrefabsOfType(entityId);
            foreach (var prefab in prefabsOfSameType)
            {
                if (!_storageService.HasBinderOfType(prefab, binderType))
                {
                    return prefab;
                }
            }

            var allPrefabs = _storageService.GetAllPrefabs();
            foreach (var prefab in allPrefabs)
            {
                var prefabManager = prefab.GetComponent<PrefabEntityManager>();
                if (prefabManager != null &&
                    prefabManager.CanHostEntityType(entityId) &&
                    !_storageService.HasBinderOfType(prefab, binderType))
                {
                    return prefab;
                }
            }

            return null;
        }

        private bool _isDisposed = false;

        public void Dispose()
        {
            if (_isDisposed) return;
            _isDisposed = true;
        }
    }
}