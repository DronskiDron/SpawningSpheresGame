using System;
using MVVMZoomCamera;
using UnityEngine;

namespace SpawningSpheresGame.Game.Settings.DataStorage
{
    [Serializable]
    public class ZoomRtsCameraPreset
    {
        public CameraPresetId Id;
        [SerializeField] private ZoomProperties _zoomProperties;

        public ZoomProperties ZoomProperties { get => _zoomProperties; }
    }
}