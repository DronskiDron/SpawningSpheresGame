using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpawningSpheresGame.Game.Settings.DataStorage
{
    [Serializable]
    public class MoveRtsCameraConfig : BaseConfig
    {
        [SerializeField] private List<MoveRtsCameraPreset> _cameraPresetList;

        public IReadOnlyList<MoveRtsCameraPreset> CameraPresetList { get => _cameraPresetList; }


        public MoveRtsCameraPreset GetCameraPresetById(CameraPresetId id)
        {
            foreach (var cameraPreset in _cameraPresetList)
            {
                if (cameraPreset.Id == id) return cameraPreset;
            }

            throw new NullReferenceException("There is no suitable camera preset in storage!!!");
        }
    }
}