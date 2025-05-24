using System;
using SpawningSpheresGame.Game.State.DataTypes;

namespace SpawningSpheresGame.Game.State.Entities.Creatures.Player
{
    [Serializable]
    public class PlayerEntityData : CreatureEntityData
    {
        public TransformStateData PlayerCameraOffsetData = new();


        public PlayerEntityData()
        {
            Type = EntityType.PlayerEntity;
        }
    }
}