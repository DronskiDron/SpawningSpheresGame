using System;
using System.Collections.Generic;
using SpawningSpheresGame.Game.State.Entities;
using SpawningSpheresGame.Game.State.Entities.Cameras;
using SpawningSpheresGame.Game.State.Entities.Creatures;
using SpawningSpheresGame.Game.State.Entities.Creatures.Player;
using SpawningSpheresGame.Game.State.Entities.Test;
using UnityEngine;

namespace SpawningSpheresGame.Utils.GameStateUtils
{
    public static class GameStateEntityFactory
    {
        private static readonly Dictionary<EntityType, Func<EntityData, IEntity>> EntityCreators =
            new Dictionary<EntityType, Func<EntityData, IEntity>>
        {
    { EntityType.PlayerEntity, data => {
        if (data is PlayerEntityData playerData)
            return new PlayerEntity(playerData);
        throw new ArgumentException($"Expected PlayerEntityData but got {data.GetType().Name}");
    }},
    { EntityType.AdvancedPlayerEntity, data => {
        if (data is AdvancedPlayerEntityData advancedPlayerData)
        return new AdvancedPlayerEntity(advancedPlayerData);
        throw new ArgumentException($"Expected AdvancedPlayerEntityData but got {data.GetType().Name}");
    }},
    { EntityType.CreatureEntity, data => {
        if (data is CreatureEntityData creatureData)
            return new CreatureEntity(creatureData);
        throw new ArgumentException($"Expected CreatureEntityData but got {data.GetType().Name}");
    }},
    { EntityType.TestEntity, data => {
        if (data is TestEntityData testData)
            return new TestEntity(testData);
        throw new ArgumentException($"Expected TestEntityData but got {data.GetType().Name}");
    }},
    { EntityType.ZoomRtsCameraEntity, data => {
        if (data is ZoomRtsCameraEntityData zoomCameraData)
            return new ZoomRtsCameraEntity(zoomCameraData);
        throw new ArgumentException($"Expected ZoomRtsCameraEntityData but got {data.GetType().Name}");
    }},
    { EntityType.MoveRtsCameraEntity, data => {
        if (data is MoveRtsCameraEntityData zoomCameraData)
            return new MoveRtsCameraEntity(zoomCameraData);
        throw new ArgumentException($"Expected ZoomRtsCameraEntityData but got {data.GetType().Name}");
    }},
    { EntityType.BaseEntity, data => new Entity(data) }
        };

        public static IEntity CreateEntity(EntityData data)
        {
            if (data == null) return null;

            try
            {
                if (EntityCreators.TryGetValue(data.Type, out var creator))
                {
                    return creator(data);
                }

                return new Entity(data);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to create entity of type {data.Type}: {ex.Message}");
                return new Entity(data);
            }
        }

        public static T CreateEntity<T>(EntityData data) where T : class, IEntity
        {
            var entity = CreateEntity(data);
            if (entity is T typedEntity)
                return typedEntity;

            throw new InvalidCastException($"Cannot convert {entity.GetType().Name} to {typeof(T).Name}");
        }
    }
}