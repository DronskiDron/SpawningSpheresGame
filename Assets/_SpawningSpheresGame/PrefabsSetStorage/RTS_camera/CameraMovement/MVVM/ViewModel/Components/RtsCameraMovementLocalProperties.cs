using UnityEngine;

namespace MVVMCameraMovement
{
    public class RtsCameraMovementLocalProperties
    {
        public bool DragEnabled { get; set; }
        public Vector3 PreviousMousePosition { get; set; }
        public Vector3 MouseDelta { get; set; }
        public Vector2 KeyboardInput { get; set; }
        public float MouseSensitivity { get; set; }
        public float KeyboardSensitivity { get; set; }
        public float RotationSpeed { get; set; }
        public float RotationInput { get; set; }
    }
}