using System;
using UnityEngine;

namespace SpawningSpheresGame.Game.Settings.DataStorage
{
    [Serializable]
    public class PrefabDataEntry
    {
        public PrefabEntityType Type;
        [SerializeReference] public BasePrefabData Data;
    }
}