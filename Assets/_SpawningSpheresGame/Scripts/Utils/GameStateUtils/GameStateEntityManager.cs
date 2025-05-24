using System;
using System.Linq;
using ObservableCollections;
using SpawningSpheresGame.Game.State.Entities;
using SpawningSpheresGame.Game.State.Entities.Cameras;
using SpawningSpheresGame.Game.State.Entities.Creatures;
using SpawningSpheresGame.Game.State.Entities.Test;
using SpawningSpheresGame.Game.State.Root;
using UnityEngine;

namespace SpawningSpheresGame.Utils.GameStateUtils
{
    public static class GameStateEntityManager
    {
        public static bool AddEntityToGameState(IEntity<EntityData> entity, GameState gameState)
        {
            if (entity == null || gameState == null)
                return false;

            try
            {
                switch (entity.Type)
                {
                    case EntityType.PlayerEntity:
                    case EntityType.CreatureEntity:
                    case EntityType.AdvancedPlayerEntity:
                        if (entity is ICreatureEntity creatureEntity)
                        {
                            gameState.Creatures.Add(creatureEntity);
                            return true;
                        }
                        break;
                    case EntityType.TestEntity:
                        if (entity is ITestEntity testEntity)
                        {
                            gameState.CustomEntities.Add(testEntity);
                            return true;
                        }
                        break;
                    case EntityType.ZoomRtsCameraEntity:
                        if (entity is IZoomRtsCameraEntity zoomCameraEntity)
                        {
                            gameState.CustomEntities.Add(zoomCameraEntity);
                            return true;
                        }
                        break;
                    case EntityType.MoveRtsCameraEntity:
                        if (entity is IMoveRtsCameraEntity moveCameraEntity)
                        {
                            gameState.CustomEntities.Add(moveCameraEntity);
                            return true;
                        }
                        break;
                    default:
                        gameState.CustomEntities.Add((IEntity)entity);
                        return true;
                }
                Debug.LogWarning($"Entity of type {entity.Type} could not be added to GameState");
                return false;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error adding entity to GameState: {ex.Message}");
                return false;
            }
        }

        public static bool RemoveEntityFromGameState(int entityId, GameState gameState)
        {
            if (gameState == null)
                return false;

            try
            {
                var creatureToRemove = FindEntityInCollection(gameState.Creatures, entityId);
                if (creatureToRemove != null)
                {
                    gameState.Creatures.Remove(creatureToRemove);
                    return true;
                }

                var customEntityToRemove = FindEntityInCollection(gameState.CustomEntities, entityId);
                if (customEntityToRemove != null)
                {
                    gameState.CustomEntities.Remove(customEntityToRemove);
                    return true;
                }

                Debug.LogWarning($"Entity with ID {entityId} not found in any GameState collection");
                return false;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error removing entity from GameState: {ex.Message}");
                return false;
            }
        }

        public static T FindEntityInCollection<T>(ObservableList<T> collection, int entityId) where T : IEntityBase
        {
            return collection.FirstOrDefault(e => e.TempId.Value == entityId);
        }
    }
}