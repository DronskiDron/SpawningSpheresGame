using System;

namespace SpawningSpheresGame.Game.State.Entities.Cameras
{
    [Serializable]
    public class ZoomRtsCameraEntityData : EntityData
    {
        public float FieldOfView { get; set; }
        public float OrthographicSize { get; set; }

        public ZoomRtsCameraEntityData()
        {
            Type = EntityType.ZoomRtsCameraEntity;
        }
    }
}