using UnityEngine;

namespace SpawningSpheresGame.Game.Settings.DataStorage
{
    [CreateAssetMenu(fileName = "MainStorage", menuName = "Game Settings/Storage/New MainStorage")]
    public class MainStorage : ScriptableObject
    {
        public PrefabsStorage PrefabsStorage;
        public ConfigsStorage ConfigsStorage;
    }
}
