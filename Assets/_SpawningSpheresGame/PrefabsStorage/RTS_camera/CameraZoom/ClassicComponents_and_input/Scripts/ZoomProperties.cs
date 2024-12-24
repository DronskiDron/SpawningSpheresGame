using System;

namespace SpawningSpheresGame.PrefabPrefabsStorage.RTS_camera.CameraZoom.ClassicComponents_and_input
{
    [Serializable]
    public class ZoomProperties
    {
        public float ZoomSpeed = 1;
        public float Smoothness = 1;
        public float ZoomMin = 3;
        public float ZoomMax = 10;
    }
}