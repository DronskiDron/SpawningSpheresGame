using System.Collections.Generic;
using UnityEngine;

namespace SpawningSpheresGame.Game.Settings.DataStorage
{
    [CreateAssetMenu(fileName = "PrefabsStorage", menuName = "Game Settings/Storage/New PrefabsStorage")]
    public class PrefabsStorage : ScriptableObject
    {
        [SerializeField] private List<StoredPrefabTotalData> _prefabsList;

        public IReadOnlyList<StoredPrefabTotalData> PrefabsList { get => _prefabsList; }
    }
}
