using SpawningSpheresGame.Game.Settings.DataStorage;
using UnityEngine;

namespace SpawningSpheresGame.Game.Settings
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "Game Settings/New Game Settings")]
    public class GameSettings : ScriptableObject
    {
        public MainStorage MainStorage;
    }
}