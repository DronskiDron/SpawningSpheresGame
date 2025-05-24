using System;
using SpawningSpheresGame.Game.State;
using SpawningSpheresGame.Game.State.Entities;
using SpawningSpheresGame.Utils;
using SpawningSpheresGame.Utils.GameStateUtils;
using Zenject;

namespace SpawningSpheresGame.Game.Gameplay.Services
{
    public class EntityStateService : IDisposable
    {
        private readonly IGameStateProvider _gameStateProvider;
        private readonly IdGenerator _idGenerator = new();

        public EntityStateService(DiContainer container)
        {
            _gameStateProvider = container.Resolve<IGameStateProvider>();
        }

        public int GenerateId() => _idGenerator.GenerateId();

        public void AssignTempId(IEntity<EntityData> entity, int mapId)
        {
            entity.TempId.Value = mapId;
        }

        public void AddEntityToGameState(IEntity<EntityData> entity)
        {
            if (!GameStateEntityManager.AddEntityToGameState(entity, _gameStateProvider.GameState))
            {
                throw new InvalidOperationException($"Failed to add entity of type {entity.Type} to GameState");
            }
        }

        public void RemoveEntityFromGameState(int mapId)
        {
            GameStateEntityManager.RemoveEntityFromGameState(mapId, _gameStateProvider.GameState);
        }

        public void SaveGameState()
        {
            _gameStateProvider.SaveGameState();
        }

        private bool _isDisposed = false;

        public void Dispose()
        {
            if (_isDisposed) return;
            _isDisposed = true;
        }
    }
}