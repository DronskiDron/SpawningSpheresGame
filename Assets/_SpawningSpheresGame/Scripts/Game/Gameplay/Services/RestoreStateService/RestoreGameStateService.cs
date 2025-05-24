using System;
using ObservableCollections;
using SpawningSpheresGame.Game.State;
using SpawningSpheresGame.Game.State.Entities;
using UnityEngine;
using Zenject;

namespace SpawningSpheresGame.Game.Gameplay.Services
{
    public class RestoreGameStateService : IDisposable
    {
        private readonly IGameStateProvider _gameStateProvider;
        private readonly PrefabInstanceIdService _restoreListService;
        private readonly GameplayEntityService _gameplayEntityService;

        public RestoreGameStateService(DiContainer container)
        {
            _gameStateProvider = container.Resolve<IGameStateProvider>();
            _restoreListService = container.Resolve<PrefabInstanceIdService>();
            _gameplayEntityService = container.Resolve<GameplayEntityService>();
        }

        public void RestoreAllEntities()
        {
            try
            {
                RestoreList(_gameStateProvider.GameState.Creatures);
                RestoreList(_gameStateProvider.GameState.CustomEntities);

                _gameStateProvider.SaveGameState();
            }
            catch (Exception ex)
            {
                Debug.LogError($"Ошибка восстановления: {ex.Message}\n{ex.StackTrace}");
            }
        }

        public void RestoreList(ObservableList<IEntity> list)
        {
            foreach (var entity in list)
            {
                if (!_restoreListService.CheckPrefabInstanceIdIsAlreadyExists(entity.PrefabInstanceId.Value))
                {
                    _gameplayEntityService.RestoreEntity(entity);
                }
                else
                {
                    _gameplayEntityService.RestoreEntityOnPrefab(entity
                        , _restoreListService.GetAlreadyExistingPrefabByPrefabInstanceId(entity.PrefabInstanceId.Value));
                }
            }
        }

        private bool _isDisposed = false;

        public void Dispose()
        {
            if (_isDisposed) return;
            _isDisposed = true;
        }
    }
}