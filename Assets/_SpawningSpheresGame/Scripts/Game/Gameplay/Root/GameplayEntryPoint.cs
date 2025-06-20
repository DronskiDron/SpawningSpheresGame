using R3;
using SpawningSpheresGame.Game.Common;
using SpawningSpheresGame.Game.Gameplay.Root.GameplayParams;
using SpawningSpheresGame.Game.Gameplay.Root.View;
using SpawningSpheresGame.Game.GameRoot;
using SpawningSpheresGame.Game.GameRoot.RootManagers;
using SpawningSpheresGame.Game.MainMenu.Root.MainMenuParams;
using SpawningSpheresGame.Scripts.Game.Gameplay.View.UI;
using UnityEngine;
using Zenject;

namespace SpawningSpheresGame.Game.Gameplay.Root
{
    public class GameplayEntryPoint : MonoBehaviour
    {
        [SerializeField] private UIGameplayRootBinder _sceneUIRootPrefab;
        [SerializeField] private WorldGameplayRootBinder _worldRootBinder;

        private TickableManager _tickManager;
        private RootSceneContainersManager _sceneContainersManager;
        private readonly CompositeDisposable _subscriptions = new();

        public Observable<GameplayExitParams> Run(DiContainer gameplayContainer, GameplayEnterParams enterParams)
        {
            GameplayRegistrations.Register(gameplayContainer, enterParams, _subscriptions);
            InitTickManager(gameplayContainer);

            var gameplayViewModelsContainer = new DiContainer(gameplayContainer);
            GameplayViewModelsRegistrations.Register(gameplayViewModelsContainer, _subscriptions);

            _sceneContainersManager = gameplayContainer.Resolve<RootSceneContainersManager>();
            _sceneContainersManager.InitWorldContainers(_worldRootBinder.WorldContainerComponent.GetObjectContainers());
            // For test only!!!:
            InitWorld(gameplayViewModelsContainer);
            InitUI(gameplayViewModelsContainer);

            var mainMenuEnterParams = new MainMenuEnterParams("mainMenuEnterParamsTestString");
            var exitParams = new GameplayExitParams(mainMenuEnterParams);
            var exitSceneRequest = gameplayContainer.ResolveId<Subject<Unit>>(AppConstants.EXIT_SCENE_REQUEST_TAG);
            var exitToMainMenuSceneSignal = exitSceneRequest.Select(_ => exitParams);

            return exitToMainMenuSceneSignal;
        }

        private void InitWorld(DiContainer viewsContainer)
        {
            _worldRootBinder.Bind(viewsContainer.Resolve<WorldGameplayRootViewModel>().AddTo(_subscriptions));
        }

        private void InitUI(DiContainer viewsContainer)
        {
            var uiRoot = viewsContainer.Resolve<UIRootView>();
            var uiSceneRootBinder = Instantiate(_sceneUIRootPrefab);
            uiRoot.AttachSceneUI(uiSceneRootBinder.gameObject);

            var uiSceneRootViewModel = viewsContainer.Resolve<UIGameplayRootViewModel>().AddTo(_subscriptions);
            uiSceneRootBinder.Bind(uiSceneRootViewModel);

            var uiManager = viewsContainer.Resolve<GameplayUIManager>();
            uiManager.OpenPlayerControlScreen();
        }

        private void InitTickManager(DiContainer gameplayContainer)
        {
            _tickManager = gameplayContainer.Resolve<TickableManager>();
            _tickManager.Initialize();
        }

        private void Update()
        {
            _tickManager?.Update();
        }

        private void LateUpdate()
        {
            _tickManager?.LateUpdate();
        }

        private void FixedUpdate()
        {
            _tickManager?.FixedUpdate();
        }

        private void OnDestroy()
        {
            _subscriptions.Dispose();
        }
    }
}
