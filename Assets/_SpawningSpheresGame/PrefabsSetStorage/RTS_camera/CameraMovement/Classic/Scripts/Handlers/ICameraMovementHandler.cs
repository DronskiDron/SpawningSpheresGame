using UnityEngine;

namespace SpawningSpheresGame.PrefabPrefabsStorage.RTS_camera.CameraMovement.Classic
{
    public interface ICameraMovementHandler
    {
        void Move(Vector3 inputDelta);
    }
}