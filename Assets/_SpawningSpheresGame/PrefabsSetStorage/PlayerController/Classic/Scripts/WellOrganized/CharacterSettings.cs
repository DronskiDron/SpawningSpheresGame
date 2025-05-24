using System;
using UnityEngine;

namespace PlayerController.Classic
{
    [Serializable]
    public class CharacterSettings
    {
        [Header("Movement Settings")]
        public float MoveSpeed = 7f;
        public float SprintSpeed = 15f;
        public float MaxAcceleration = 20f;

        [Header("Look Settings")]
        public float LookSpeedX = 2f;
        public float LookSpeedY = 2f;
        public float UpperLookLimit = 80f;
        public float LowerLookLimit = -80f;

        [Header("Jump Settings")]
        public float JumpForce = 15f;
        public float MaxJumpTime = 0.5f;
        public float JumpHoldForce = 15f;

        [Header("Gravity Settings")]
        public float GravityMultiplier = 40f;
    }
}