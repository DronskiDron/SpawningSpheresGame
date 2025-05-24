using UnityEngine;

namespace MVVMCameraMovement
{
    public interface IRtsCameraMovementHandler
    {
        void Move(Vector3 inputDelta);
    }
}