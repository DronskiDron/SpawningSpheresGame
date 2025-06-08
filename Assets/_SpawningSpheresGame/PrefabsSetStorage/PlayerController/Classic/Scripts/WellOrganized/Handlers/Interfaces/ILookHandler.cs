using UnityEngine;

namespace PlayerController
{
    public interface ILookHandler
    {
        void HandleLook(Vector2 lookInput);
    }
}