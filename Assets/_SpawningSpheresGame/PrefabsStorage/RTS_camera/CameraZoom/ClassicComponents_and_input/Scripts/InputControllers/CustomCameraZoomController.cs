using UnityEngine;

namespace SpawningSpheresGame.PrefabPrefabsStorage.RTS_camera.CameraZoom.ClassicComponents_and_input
{
    public class CustomCameraZoomController : CameraZoomController
    {
        [SerializeField] private float _verticalMultiplayer = 0.01f;

        protected override float ReadInputDelta()
        {
            return Input.GetAxis("Mouse ScrollWheel") + Input.GetAxis("Vertical") * _verticalMultiplayer;
        }
    }
}