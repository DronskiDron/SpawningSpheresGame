using UnityEngine;

namespace PlayerController.MVVM.Player
{
    public class PlayerLookApplier
    {
        private readonly Rigidbody _rb;
        private readonly Transform _cameraPivot;


        public PlayerLookApplier(Rigidbody rb, Transform cameraPivot)
        {
            _rb = rb;
            _cameraPivot = cameraPivot;
        }


        public void ApplyLook(Quaternion horizontalRotation, Quaternion verticalRotation)
        {
            _rb.MoveRotation(_rb.rotation * horizontalRotation);
            _cameraPivot.localRotation = verticalRotation;
        }
    }
}