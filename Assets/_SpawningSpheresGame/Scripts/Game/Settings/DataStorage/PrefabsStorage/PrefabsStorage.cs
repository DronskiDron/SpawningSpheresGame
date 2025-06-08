using System.Collections.Generic;
using UnityEngine;

namespace SpawningSpheresGame.Game.Settings.DataStorage
{
    [CreateAssetMenu(fileName = "PrefabsStorage", menuName = "Game Settings/Storage/Prefabs Storage")]
    public class PrefabsStorage : ScriptableObject
    {
        [SerializeField] private List<PrefabTotalData> _prefabsList = new List<PrefabTotalData>();

        private Dictionary<PrefabId, PrefabTotalData> _prefabsCache;

        public IReadOnlyList<PrefabTotalData> PrefabsList => _prefabsList;


        private void OnEnable()
        {
            InitializeCache();
        }


        private void InitializeCache()
        {
            _prefabsCache = new Dictionary<PrefabId, PrefabTotalData>();

            foreach (var prefabData in _prefabsList)
            {
                if (prefabData != null)
                {
                    _prefabsCache[prefabData.Id] = prefabData;
                }
            }
        }


        public PrefabTotalData GetPrefabTotalDataById(PrefabId id)
        {
            if (_prefabsCache == null)
            {
                InitializeCache();
            }

            if (_prefabsCache.TryGetValue(id, out var prefabData))
            {
                return prefabData;
            }

            foreach (var data in _prefabsList)
            {
                if (data.Id == id)
                {
                    _prefabsCache[id] = data;
                    return data;
                }
            }

            Debug.LogError($"Prefab with ID {id} not found in storage");
            return null;
        }


        public T GetEntityDataById<T>(PrefabId prefabId, PrefabEntityType entityType) where T : BasePrefabData
        {
            var prefabData = GetPrefabTotalDataById(prefabId);
            if (prefabData == null) return null;

            return prefabData.GetEntityData<T>(entityType);
        }
    }
}
