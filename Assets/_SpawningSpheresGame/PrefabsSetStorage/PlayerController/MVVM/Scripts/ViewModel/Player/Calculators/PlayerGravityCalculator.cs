using PlayerController.Classic;
using UnityEngine;

namespace PlayerController.MVVM.Player
{
    public class PlayerGravityCalculator
    {
        private readonly CharacterSettings _settings;


        public PlayerGravityCalculator(CharacterSettings settings)
        {
            _settings = settings;
        }


        public Vector3 CalculateGravityForce(bool isGrounded)
        {
            if (!isGrounded)
            {
                return Vector3.down * _settings.GravityMultiplier;
            }
            return Vector3.zero;
        }
    }
}