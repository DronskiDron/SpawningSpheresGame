using System;
using UnityEngine;

namespace SpawningSpheresGame.Game.Settings.DataStorage
{
    [Serializable]
    public class MoveCameraData : BasePrefabData
    {
        [SerializeField] private StoredTransformStateData _startTransform;

        public StoredTransformStateData StartTransform => _startTransform;
    }
}