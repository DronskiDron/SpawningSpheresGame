using System;
using SpawningSpheresGame.Game.Common.DataTypes.Enums;
using SpawningSpheresGame.Game.State.Entities;

namespace SpawningSpheresGame.Utils.GameplayUtils
{
    public static class EntityTypeToGameplayEntityConverter
    {
        public static GameplayEntitiesId Convert(EntityType entityType)
        {
            return entityType switch
            {
                EntityType.TestEntity => GameplayEntitiesId.Test,
                EntityType.PlayerEntity => GameplayEntitiesId.Player,
                EntityType.ZoomRtsCameraEntity => GameplayEntitiesId.ZoomRtsCamera,
                EntityType.MoveRtsCameraEntity => GameplayEntitiesId.MoveRtsCamera,
                EntityType.AdvancedPlayerEntity => GameplayEntitiesId.AdvancedPlayer,
                _ => throw new ArgumentException($"Unsupported entity type: {entityType}")
            };
        }
    }
}