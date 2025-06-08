using System;
using System.Collections.Generic;

namespace SpawningSpheresGame.Game.State.Entities.Creatures.Player
{
    [Serializable]
    public class AdvancedPlayerEntityData : PlayerEntityData
    {
        public float SpecialPower { get; set; }
        public int ExtraLives { get; set; }
        public List<string> Abilities { get; set; } = new();

        public AdvancedPlayerEntityData()
        {
            Type = EntityType.AdvancedPlayerEntity;
        }
    }
}