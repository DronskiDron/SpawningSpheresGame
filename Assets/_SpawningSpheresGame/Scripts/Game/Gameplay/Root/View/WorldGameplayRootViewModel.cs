using System;
using SpawningSpheresGame.Game.Common.DataTypes.Enums;
using SpawningSpheresGame.Game.Gameplay.Services;
using SpawningSpheresGame.Game.GameRoot.RootManagers;
using SpawningSpheresGame.Game.State;
using Zenject;

namespace SpawningSpheresGame.Game.Gameplay.Root.View
{
    public class WorldGameplayRootViewModel : IDisposable
    {
        private DiContainer _container;
        private IGameStateProvider _gameStateProvider;
        private GameplayEntityService _gameplayEntityService;
        private RestoreGameStateService _restoreGameStateService;

        public WorldGameplayRootViewModel(DiContainer container)
        {
            _container = container;

            _gameStateProvider = _container.Resolve<IGameStateProvider>();
            _gameplayEntityService = _container.Resolve<GameplayEntityService>();
            _restoreGameStateService = _container.Resolve<RestoreGameStateService>();
        }

        public void CreateEntity(GameplayEntitiesId gameplayEntitiesId, ObjectContainersEnum objectContainersEnum)
        {
            _gameplayEntityService.CreateEntityWithExistingPrefabIfPossible(gameplayEntitiesId, objectContainersEnum);
        }

        public void DeleteEntity(int tempId)
        {
            _gameplayEntityService.DeleteEntity(tempId);
        }

        public void RestoreGameState()
        {
            if (_gameStateProvider.GameState != null)
            {
                _restoreGameStateService.RestoreAllEntities();
            }
        }

        public void SaveGameState()
        {
            _gameStateProvider.SaveGameState();
        }

        public void Dispose()
        {
            _gameplayEntityService = null;
            _restoreGameStateService = null;
            _gameStateProvider = null;
            _container = null;
        }
    }
}