using R3;
using SpawningSpheresGame.Game.Common;
using SpawningSpheresGame.Game.Gameplay.Root.GameplayParams;
using SpawningSpheresGame.Game.Gameplay.Services;
using Zenject;

namespace SpawningSpheresGame.Game.Gameplay.Root
{
    public static class GameplayRegistrations
    {
        public static void Register(DiContainer container, GameplayEnterParams gameplayEnterParams
                        , CompositeDisposable subscriptions)
        {
            container.Bind<TickableManager>().AsSingle();
            container.BindInstance(new Subject<Unit>()).WithId(AppConstants.EXIT_SCENE_REQUEST_TAG);

            var gameplayEntityService = new GameplayEntityService(container).AddTo(subscriptions);
            container.BindInstance(gameplayEntityService).AsSingle();

            var restoreGameStateService = new RestoreGameStateService(container).AddTo(subscriptions);
            container.BindInstance(restoreGameStateService).AsSingle();
        }
    }
}