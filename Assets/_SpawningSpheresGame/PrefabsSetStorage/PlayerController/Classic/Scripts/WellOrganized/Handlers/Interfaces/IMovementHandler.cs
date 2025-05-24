using UnityEngine;

namespace PlayerController
{
    public interface IMovementHandler
    {
        void Move(Vector2 input, bool isSprinting);
    }
}