using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpawningSpheresGame.Game.Settings.DataStorage
{
    [Serializable]
    public class ZoomRtsCameraConfig : BaseConfig
    {
        [SerializeField] private List<ZoomRtsCameraPreset> _cameraPresetList;

        public IReadOnlyList<ZoomRtsCameraPreset> CameraPresetList { get => _cameraPresetList; }


        public ZoomRtsCameraPreset GetCameraPresetById(CameraPresetId id)
        {
            foreach (var cameraPreset in _cameraPresetList)
            {
                if (cameraPreset.Id == id) return cameraPreset;
            }

            throw new NullReferenceException("There is no suitable camera preset in storage!!!");
        }
    }
}