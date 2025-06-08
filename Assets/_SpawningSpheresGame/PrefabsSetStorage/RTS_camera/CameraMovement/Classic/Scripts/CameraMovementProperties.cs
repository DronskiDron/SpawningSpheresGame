using System;
using UnityEngine;

namespace SpawningSpheresGame.PrefabPrefabsStorage.RTS_camera.CameraMovement.Classic
{
    [Serializable]
    public class CameraMovementProperties
    {
        public Transform Pivot;
        public float Speed = 4;
        public float Smoothness = 0.2f;
    }
}