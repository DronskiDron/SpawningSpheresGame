using SpawningSpheresGame.PrefabPrefabsStorage.RTS_camera.CameraMovement.Classic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SpawningSpheresGame.PrefabPrefabsStorage.RTS_camera.CameraMovement.NewInput
{
    [RequireComponent(typeof(Camera))]
    public class CustomCameraMovementNewInput : CameraMovementInputBase
    {
        [SerializeField] private LayerMask _raycastMask;
        [SerializeField] private LayerMask _groundMask;
        [SerializeField] private float _mouseSensitive = 1f;
        [SerializeField] private float _keyboardSensitive = 1f;

        private Camera _camera;
        private ApplicationInputController _cameraInput;
        private bool _dragEnabled;
        private Vector3 _previousMousePosition;
        private Vector2 _movementInput;


        protected override void Awake()
        {
            base.Awake();
            _camera = GetComponent<Camera>();
            _cameraInput = new ApplicationInputController();
        }


        private void OnEnable()
        {
            _cameraInput.Enable();
            _cameraInput.Camera.Drag.performed += OnDragPerformed;
            _cameraInput.Camera.Drag.canceled += OnDragCanceled;
            _cameraInput.Camera.Movement.performed += OnMovementPerformed;
            _cameraInput.Camera.Movement.canceled += OnMovementCanceled;
        }


        private void OnDisable()
        {
            _cameraInput.Camera.Drag.performed -= OnDragPerformed;
            _cameraInput.Camera.Drag.canceled -= OnDragCanceled;
            _cameraInput.Camera.Movement.performed -= OnMovementPerformed;
            _cameraInput.Camera.Movement.canceled -= OnMovementCanceled;
            _cameraInput.Disable();
        }


        private void OnDragPerformed(InputAction.CallbackContext context)
        {
            if (IsClickedOnGround())
            {
                _dragEnabled = true;
                _previousMousePosition = Input.mousePosition;
            }
        }


        private void OnDragCanceled(InputAction.CallbackContext context)
        {
            _dragEnabled = false;
        }


        private void OnMovementPerformed(InputAction.CallbackContext context)
        {
            _movementInput = context.ReadValue<Vector2>();
        }


        private void OnMovementCanceled(InputAction.CallbackContext context)
        {
            _movementInput = Vector2.zero;
        }


        protected override ICameraMovementHandler CreateMovementHandler()
        {
            return new SmoothCameraMovementHandler(_properties);
        }


        protected override Vector3 ReadInputDelta()
        {
            var mouseInputDelta = ReadMouseInputDelta();
            var keyboardInputDelta = ReadKeyboardInputDelta();

            return (mouseInputDelta + keyboardInputDelta) * _camera.fieldOfView;
        }


        private Vector3 ReadMouseInputDelta()
        {
            if (!_dragEnabled) return Vector3.zero;

            Vector3 currentMousePosition = Input.mousePosition;
            Vector3 delta = (currentMousePosition - _previousMousePosition) * _mouseSensitive;
            _previousMousePosition = currentMousePosition;

            return delta;
        }


        private Vector3 ReadKeyboardInputDelta()
        {
            return new Vector3(-_movementInput.x, -_movementInput.y, 0) * _keyboardSensitive;
        }


        private bool IsClickedOnGround()
        {
            var pointerScreenPosition = Input.mousePosition;
            var ray = _camera.ScreenPointToRay(pointerScreenPosition);
            var result = Physics.Raycast(ray, out var hit, float.MaxValue, _raycastMask.value);

            if (!result) return false;

            return IsLayerInMask(hit.collider.gameObject.layer, _groundMask);
        }


        private bool IsLayerInMask(int layer, LayerMask mask)
        {
            return (mask.value & (1 << layer)) != 0;
        }
    }
}
