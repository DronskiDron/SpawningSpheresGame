using System;

namespace SpawningSpheresGame.Game.State.Entities.Cameras
{
    [Serializable]
    public class MoveRtsCameraEntityData : EntityData
    {
        public MoveRtsCameraEntityData()
        {
            Type = EntityType.MoveRtsCameraEntity;
        }
    }
}