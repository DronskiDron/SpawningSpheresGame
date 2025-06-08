using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpawningSpheresGame.Game.Settings.DataStorage
{
    [Serializable]
    public class PrefabTotalData
    {
        [SerializeField] private PrefabId _id;
        [SerializeField] private GameObject _prefab;
        [SerializeField] private List<TransformPreset> _transformPresets = new List<TransformPreset>();
        [SerializeField] public List<PrefabDataEntry> CustomData = new List<PrefabDataEntry>();

        public PrefabId Id => _id;
        public GameObject Prefab => _prefab;
        public IReadOnlyList<TransformPreset> TransformPresets => _transformPresets;


        public TransformPreset GetTransformPresetByIndex(int index = 0)
        {
            if (_transformPresets != null && index < _transformPresets.Count)
                return _transformPresets[index];

            return new TransformPreset();
        }


        public T GetEntityData<T>(PrefabEntityType entityType) where T : BasePrefabData
        {
            foreach (var entry in CustomData)
            {
                if (entry.Type == entityType && entry.Data is T typedData)
                    return typedData;
            }

            Debug.LogWarning($"Entity data for type {entityType} not found in prefab {_id}");
            return null;
        }
    }
}