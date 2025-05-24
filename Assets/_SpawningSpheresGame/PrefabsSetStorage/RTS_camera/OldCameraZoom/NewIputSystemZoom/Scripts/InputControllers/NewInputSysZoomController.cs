using SpawningSpheresGame.PrefabPrefabsStorage.RTS_camera.CameraZoom.ClassicComponents_and_input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SpawningSpheresGame.PrefabPrefabsStorage.RTS_camera.CameraZoom.NewIputSystemZoom
{
    public class NewInputSysZoomController : CameraZoomController
    {
        [SerializeField] private float _verticalMultiplayer = 0.01f;
        private ApplicationInputController _cameraInput;
        private float _zoomInput;


        protected override void Awake()
        {
            base.Awake();
            _cameraInput = new ApplicationInputController();
        }


        private void OnEnable()
        {
            _cameraInput.Enable();
            _cameraInput.Camera.Zoom.performed += OnZoomPerformed;
            _cameraInput.Camera.Zoom.canceled += OnZoomCanceled;
        }


        private void OnDisable()
        {
            _cameraInput.Camera.Zoom.performed -= OnZoomPerformed;
            _cameraInput.Camera.Zoom.canceled -= OnZoomCanceled;
            _cameraInput.Disable();
        }


        private void OnZoomPerformed(InputAction.CallbackContext context)
        {
            _zoomInput = context.ReadValue<float>();
        }


        private void OnZoomCanceled(InputAction.CallbackContext context)
        {
            _zoomInput = 0f;
        }


        protected override float ReadInputDelta()
        {
            return _zoomInput * _verticalMultiplayer;
        }
    }
}
