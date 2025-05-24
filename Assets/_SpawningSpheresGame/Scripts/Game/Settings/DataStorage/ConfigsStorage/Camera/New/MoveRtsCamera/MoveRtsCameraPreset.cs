using System;
using MVVMCameraMovement;
using UnityEngine;

namespace SpawningSpheresGame.Game.Settings.DataStorage
{
    [Serializable]
    public class MoveRtsCameraPreset
    {
        public CameraPresetId Id;
        [SerializeField] private CameraMovementProperties _cameraMovementProperties;

        public CameraMovementProperties CameraMovementProperties { get => _cameraMovementProperties; }
    }
}