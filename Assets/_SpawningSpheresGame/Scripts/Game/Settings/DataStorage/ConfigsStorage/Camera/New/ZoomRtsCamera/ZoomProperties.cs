using System;

namespace MVVMZoomCamera
{
    [Serializable]
    public class ZoomProperties
    {
        public float ZoomSpeed = 5;
        public float Smoothness = 0.2f;
        public float ZoomMin = 3;
        public float ZoomMax = 60;
        public float VerticalMultiplayer = 0.01f;
    }
}