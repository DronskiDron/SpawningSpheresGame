using UnityEngine;

namespace SpawningSpheresGame.PrefabPrefabsStorage.RTS_camera.CameraZoom.ClassicComponents_and_input
{
    public class ScrollWheelCameraZoomController : CameraZoomController
    {
        protected override float ReadInputDelta()
        {
            return Input.GetAxis("Mouse ScrollWheel");
        }
    }
}