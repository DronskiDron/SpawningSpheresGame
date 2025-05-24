using R3;
using SpawningSpheresGame.Scripts.Game.Gameplay.View.UI;
using Zenject;

namespace SpawningSpheresGame.Game.Gameplay.Root.View
{
    public static class GameplayViewModelsRegistrations
    {
        public static void Register(DiContainer container, CompositeDisposable subscriptions)
        {
            var gameplayUIManager = new GameplayUIManager(container);
            container.BindInstance(gameplayUIManager).AsSingle();

            var uIGameplayRootViewModel = new UIGameplayRootViewModel(container).AddTo(subscriptions);
            container.BindInstance(uIGameplayRootViewModel).AsSingle();

            var worldGameplayRootViewModel = new WorldGameplayRootViewModel(container).AddTo(subscriptions);
            container.BindInstance(worldGameplayRootViewModel).AsSingle();
        }
    }
}