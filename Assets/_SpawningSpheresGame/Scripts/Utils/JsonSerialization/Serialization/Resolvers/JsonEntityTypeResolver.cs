using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SpawningSpheresGame.Game.State.Entities;
using SpawningSpheresGame.Game.State.Entities.Cameras;
using SpawningSpheresGame.Game.State.Entities.Creatures;
using SpawningSpheresGame.Game.State.Entities.Creatures.Player;
using SpawningSpheresGame.Game.State.Entities.Test;

namespace SpawningSpheresGame.Utils.JsonSerialization
{
    public static class JsonEntityTypeResolver
    {
        public static EntityData ParseByType(JObject jsonObject, JsonSerializer jsonSerializer)
        {
            var type = jsonObject["Type"].ToObject<EntityType>();

            return type switch
            {
                EntityType.BaseEntity => jsonObject.ToObject<EntityData>(jsonSerializer),
                EntityType.CreatureEntity => jsonObject.ToObject<CreatureEntityData>(jsonSerializer),
                EntityType.PlayerEntity => jsonObject.ToObject<PlayerEntityData>(jsonSerializer),
                EntityType.AdvancedPlayerEntity => jsonObject.ToObject<AdvancedPlayerEntityData>(jsonSerializer),
                EntityType.TestEntity => jsonObject.ToObject<TestEntityData>(jsonSerializer),
                EntityType.ZoomRtsCameraEntity => jsonObject.ToObject<ZoomRtsCameraEntityData>(jsonSerializer),
                EntityType.MoveRtsCameraEntity => jsonObject.ToObject<MoveRtsCameraEntityData>(jsonSerializer),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}