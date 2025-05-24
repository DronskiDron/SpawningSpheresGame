using System;
using System.Linq;
using SpawningSpheresGame.Game.State.Entities;
using SpawningSpheresGame.Game.State.Entities.Cameras;
using SpawningSpheresGame.Game.State.Entities.Creatures;
using SpawningSpheresGame.Game.State.Entities.Creatures.Player;
using SpawningSpheresGame.Game.State.Entities.Test;
using SpawningSpheresGame.Game.State.Root;
using UnityEngine;

namespace SpawningSpheresGame.Utils.GameStateUtils
{
    public static class EntityStateHelper
    {
        public static bool TryAddToGameState(IEntity entity, GameStateData gameStateData)
        {
            if (entity == null || gameStateData == null) return false;

            if (TryAddToCreatures(entity, gameStateData)) return true;
            if (TryAddToCustomEntities(entity, gameStateData)) return true;

            Debug.LogWarning($"Entity of type {entity.GetType().Name} couldn't be added to any GameStateData list");
            return false;
        }

        public static bool TryAddToCreatures(IEntity entity, GameStateData gameStateData)
        {
            try
            {
                if (entity is ICreatureEntity creatureEntity)
                {
                    var origin = ((IEntity<CreatureEntityData>)creatureEntity).Origin;
                    gameStateData.Creatures.Add(origin);
                    return true;
                }
                else if (entity is PlayerEntity playerEntity)
                {
                    gameStateData.Creatures.Add(playerEntity.Origin);
                    return true;
                }
                else if (entity is CreatureEntity creature)
                {
                    gameStateData.Creatures.Add(creature.Origin);
                    return true;
                }
                else if (entity.Origin is CreatureEntityData creatureData)
                {
                    gameStateData.Creatures.Add(creatureData);
                    return true;
                }
                else if (entity is AdvancedPlayerEntity advancedPlayerEntity)
                {
                    gameStateData.Creatures.Add(advancedPlayerEntity.Origin);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error adding entity to Creatures: {ex.Message}");
            }

            return false;
        }

        public static bool TryAddToCustomEntities(IEntity entity, GameStateData gameStateData)
        {
            try
            {
                if (entity is TestEntity ||
                    !(entity is ICreatureEntity) &&
                    !(entity is PlayerEntity) &&
                    !(entity is CreatureEntity) &&
                    !(entity is AdvancedPlayerEntity)
                   )
                {
                    gameStateData.CustomEntities.Add(entity.Origin);
                    return true;
                }
                else if (entity is ZoomRtsCameraEntity zoomCamera)
                {
                    gameStateData.CustomEntities.Add(zoomCamera.Origin);
                    return true;
                }
                else if (entity is MoveRtsCameraEntity moveCamera)
                {
                    gameStateData.CustomEntities.Add(moveCamera.Origin);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error adding entity to CustomEntities: {ex.Message}");
            }

            return false;
        }

        public static bool TryRemoveFromCreatures(IEntity entity, GameStateData gameStateData)
        {
            try
            {
                var removedData = gameStateData.Creatures
                    .FirstOrDefault(data => data.TempId == entity.TempId.Value);

                if (removedData != null)
                {
                    gameStateData.Creatures.Remove(removedData);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error removing entity from Creatures: {ex.Message}");
            }

            return false;
        }

        public static bool TryRemoveFromCustomEntities(IEntity entity, GameStateData gameStateData)
        {
            try
            {
                var removedData = gameStateData.CustomEntities
                    .FirstOrDefault(data => data.TempId == entity.TempId.Value);

                if (removedData != null)
                {
                    gameStateData.CustomEntities.Remove(removedData);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error removing entity from CustomEntities: {ex.Message}");
            }

            return false;
        }
    }
}