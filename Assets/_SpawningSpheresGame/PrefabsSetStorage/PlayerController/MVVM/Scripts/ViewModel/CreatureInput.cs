using System;
using UnityEngine;

namespace PlayerController.MVVM
{
    [Serializable]
    public class CreatureInput
    {
        public Vector2 MoveDirection { get; set; }
        public Vector2 LookDirection { get; set; }
        public bool IsJumping { get; set; }
        public bool IsSprinting { get; set; }
        public bool IsGrounded { get; set; }


        public void Reset()
        {
            MoveDirection = Vector2.zero;
            LookDirection = Vector2.zero;
            IsJumping = false;
            IsSprinting = false;
            IsGrounded = false;
        }
    }
}