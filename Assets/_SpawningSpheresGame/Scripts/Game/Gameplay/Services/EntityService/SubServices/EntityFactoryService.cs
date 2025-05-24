using System;
using System.Collections.Generic;
using SpawningSpheresGame.Game.Common.DataTypes.Enums;
using SpawningSpheresGame.Game.Gameplay.Factories;
using SpawningSpheresGame.Game.Settings;
using SpawningSpheresGame.Game.State;
using SpawningSpheresGame.Game.State.Entities;
using SpawningSpheresGame.Utils.GameplayUtils;
using Zenject;

namespace SpawningSpheresGame.Game.Gameplay.Services
{
    public class EntityFactoryService : IDisposable
    {
        private readonly Dictionary<GameplayEntitiesId, IEntityFactory> _factories = new();
        private readonly DiContainer _container;
        private readonly GameSettings _gameSettings;
        private readonly IGameStateProvider _gameStateProvider;

        public EntityFactoryService(DiContainer container)
        {
            _container = container;
            _gameSettings = container.Resolve<ISettingsProvider>().GameSettings;
            _gameStateProvider = container.Resolve<IGameStateProvider>();

            EntityFactoryRegistry.RegisterFactories(_factories, _container, _gameSettings, _gameStateProvider);
        }

        public IEntityFactory GetFactory(GameplayEntitiesId entityId)
        {
            return EntityFactoryRegistry.GetFactory(_factories, entityId);
        }

        public IEntityFactory GetFactoryByEntityType(EntityType entityType)
        {
            var gameplayEntityId = EntityTypeToGameplayEntityConverter.Convert(entityType);
            return GetFactory(gameplayEntityId);
        }

        private bool _isDisposed = false;

        public void Dispose()
        {
            if (_isDisposed) return;
            _isDisposed = true;

            _factories.Clear();
        }
    }
}