using System;
using UnityEngine;

namespace SpawningSpheresGame.Game.Settings.DataStorage
{
    [Serializable]
    public class PlayerData : BasePrefabData
    {
        [SerializeField] private StoredTransformStateData _startTransform;
        [SerializeField] private StoredTransformStateData _startCameraOffset;

        public StoredTransformStateData StartTransform => _startTransform;
        public StoredTransformStateData StartCameraOffset => _startCameraOffset;
    }
}