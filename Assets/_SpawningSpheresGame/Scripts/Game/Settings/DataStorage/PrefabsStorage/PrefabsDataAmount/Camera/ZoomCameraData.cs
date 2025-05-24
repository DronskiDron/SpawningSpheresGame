using System;
using UnityEngine;

namespace SpawningSpheresGame.Game.Settings.DataStorage
{
    [Serializable]
    public class ZoomCameraData : MoveCameraData
    {
        [SerializeField] private StoredCameraZoomData _startZoomValue;

        public StoredCameraZoomData StartZoomValue => _startZoomValue;
    }
}