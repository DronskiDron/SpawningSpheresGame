using System;

namespace SpawningSpheresGame.Game.State.Entities.Creatures
{
    [Serializable]
    public class CreatureEntityData : EntityData
    {
        public CreatureEntityData()
        {
            Type = EntityType.CreatureEntity;
        }
    }
}