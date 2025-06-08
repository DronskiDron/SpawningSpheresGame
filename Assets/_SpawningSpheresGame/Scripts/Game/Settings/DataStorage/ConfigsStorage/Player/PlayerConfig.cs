using System;
using UnityEngine;

namespace SpawningSpheresGame.Game.Settings.DataStorage
{
    [Serializable]
    public class PlayerConfig : CreatureConfig
    {
        [Header("Player Specific Settings")]
        [SerializeField] private float _firstPersonCameraHeight = 1.7f;
        [SerializeField] private float _interactionDistance = 3f;
        [SerializeField] private float _footstepInterval = 0.5f;
        [SerializeField] private AudioClip[] _footstepSounds;

        public float FirstPersonCameraHeight => _firstPersonCameraHeight;
        public float InteractionDistance => _interactionDistance;
        public float FootstepInterval => _footstepInterval;
        public AudioClip[] FootstepSounds => _footstepSounds;
    }
}