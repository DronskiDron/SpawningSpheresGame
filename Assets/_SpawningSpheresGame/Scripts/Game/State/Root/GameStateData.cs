using System;
using System.Collections.Generic;
using SpawningSpheresGame.Game.State.Entities;
using SpawningSpheresGame.Game.State.Entities.Creatures;

namespace SpawningSpheresGame.Game.State.Root
{
    [Serializable]
    public class GameStateData
    {
        public List<CreatureEntityData> Creatures { get; set; }
        public List<EntityData> CustomEntities { get; set; }
    }
}