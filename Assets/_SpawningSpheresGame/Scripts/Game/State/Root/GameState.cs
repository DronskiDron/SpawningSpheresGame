using R3;
using ObservableCollections;
using SpawningSpheresGame.Game.State.Entities;
using SpawningSpheresGame.Utils.GameStateUtils;

namespace SpawningSpheresGame.Game.State.Root
{
    public class GameState
    {
        public ObservableList<IEntity> Creatures { get; } = new();
        public ObservableList<IEntity> CustomEntities { get; } = new();

        public GameState(GameStateData gameStateData)
        {
            InitCreatures(gameStateData);
            InitCustomEntities(gameStateData);
        }

        private void InitCreatures(GameStateData gameStateData)
        {
            foreach (var creatureData in gameStateData.Creatures)
            {
                var entity = GameStateEntityFactory.CreateEntity(creatureData);
                Creatures.Add(entity);
            }

            Creatures.ObserveAdd().Subscribe(e =>
                EntityStateHelper.TryAddToCreatures(e.Value, gameStateData));

            Creatures.ObserveRemove().Subscribe(e =>
                EntityStateHelper.TryRemoveFromCreatures(e.Value, gameStateData));
        }

        private void InitCustomEntities(GameStateData gameStateData)
        {
            foreach (var customEntityData in gameStateData.CustomEntities)
            {
                CustomEntities.Add(GameStateEntityFactory.CreateEntity(customEntityData));
            }

            CustomEntities.ObserveAdd().Subscribe(e =>
                EntityStateHelper.TryAddToCustomEntities(e.Value, gameStateData));

            CustomEntities.ObserveRemove().Subscribe(e =>
                EntityStateHelper.TryRemoveFromCustomEntities(e.Value, gameStateData));
        }
    }
}