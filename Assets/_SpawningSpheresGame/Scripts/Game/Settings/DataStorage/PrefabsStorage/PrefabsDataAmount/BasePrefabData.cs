using System;
using UnityEngine;

namespace SpawningSpheresGame.Game.Settings.DataStorage
{
    [Serializable]
    public abstract class BasePrefabData
    {
        [HideInInspector] public PrefabEntityType Type;
    }

    public enum PrefabEntityType
    {
        None,
        Player,
        MoveCamera,
        ZoomCamera
    }
}