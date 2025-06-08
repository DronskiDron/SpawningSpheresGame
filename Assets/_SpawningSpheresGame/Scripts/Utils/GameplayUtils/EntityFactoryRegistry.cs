using System;
using System.Collections.Generic;
using SpawningSpheresGame.Game.Common.DataTypes.Enums;
using SpawningSpheresGame.Game.Gameplay.Factories;
using SpawningSpheresGame.Game.Settings;
using SpawningSpheresGame.Game.State;
using UnityEngine;
using Zenject;

namespace SpawningSpheresGame.Utils.GameplayUtils
{
    public static class EntityFactoryRegistry
    {
        private static readonly Dictionary<GameplayEntitiesId, Type> FactoryTypes = new()
    {
        { GameplayEntitiesId.Player, typeof(PlayerEntityFactory) },
        { GameplayEntitiesId.AdvancedPlayer, typeof(AdvancedPlayerEntityFactory) },
        { GameplayEntitiesId.Test, typeof(TestEntityFactory) },
        {GameplayEntitiesId.ZoomRtsCamera,typeof(ZoomRtsCameraEntityFactory)},
        {GameplayEntitiesId.MoveRtsCamera,typeof(MoveRtsCameraEntityFactory)}
    };

        public static void RegisterFactories(Dictionary<GameplayEntitiesId, IEntityFactory> factories,
                                             DiContainer container,
                                             GameSettings gameSettings,
                                             IGameStateProvider gameStateProvider)
        {
            try
            {
                foreach (var pair in FactoryTypes)
                {
                    var factoryType = pair.Value;
                    var factory = (IEntityFactory)Activator.CreateInstance(
                        factoryType,
                        container,
                        gameSettings,
                        gameStateProvider);

                    factories[pair.Key] = factory;
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error registering factories: {ex.Message}");
            }
        }

        public static IEntityFactory GetFactory(Dictionary<GameplayEntitiesId, IEntityFactory> factories,
                                                GameplayEntitiesId entityId)
        {
            if (!factories.TryGetValue(entityId, out var factory))
            {
                throw new InvalidOperationException($"No factory registered for entity type: {entityId}");
            }
            return factory;
        }
    }
}