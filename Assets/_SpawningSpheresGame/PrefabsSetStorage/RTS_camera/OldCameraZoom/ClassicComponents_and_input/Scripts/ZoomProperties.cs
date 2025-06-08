using System;

namespace SpawningSpheresGame.PrefabPrefabsStorage.RTS_camera.CameraZoom.ClassicComponents_and_input
{
    [Serializable]
    public class ZoomProperties
    {
        public float ZoomSpeed = 5;
        public float Smoothness = 0.2f;
        public float ZoomMin = 3;
        public float ZoomMax = 60;
    }
}