using System;
using UnityEngine;

namespace MVVMCameraMovement
{
    [Serializable]
    public class CameraMovementProperties
    {
        [SerializeField] private float _speed = 4;
        [SerializeField] private float _smoothness = 0.2f;
        [SerializeField] private float _mouseSensitivity = 0.02f;
        [SerializeField] private float _keyboardSensitivity = 0.06f;
        [SerializeField] private float _rotationSpeed = 30f;

        public float Speed => _speed;
        public float Smoothness => _smoothness;
        public float MouseSensitivity => _mouseSensitivity;
        public float KeyboardSensitivity => _keyboardSensitivity;
        public float RotationSpeed => _rotationSpeed;
    }
}