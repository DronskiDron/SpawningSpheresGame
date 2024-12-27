using System;
using UnityEngine;

namespace SpawningSpheresGame.Game.Settings.DataStorage
{
    [Serializable]
    public class StoredPrefabTotalData
    {
        [SerializeField] private string _id;
        [SerializeField] private GameObject _prefab;
        [SerializeField] private PrefabData _prefabData;

        public string Id { get => _id; }
        public GameObject Prefab { get => _prefab; }
        public PrefabData PrefabData { get => _prefabData; }
    }
}