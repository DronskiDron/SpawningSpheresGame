using R3;
using SpawningSpheresGame.Game.Gameplay.Root.GameplayParams;
using SpawningSpheresGame.Game.Gameplay.Root.View;
using SpawningSpheresGame.Game.GameRoot;
using SpawningSpheresGame.Game.MainMenu.Root.MainMenuParams;
using UnityEngine;
using Zenject;

namespace SpawningSpheresGame.Game.Gameplay.Root
{
    public class GameplayEntryPoint : MonoBehaviour
    {
        [SerializeField] private UIGameplayRootBinder _sceneUIRootPrefab;
        [SerializeField] private WorldGameplayRootBinder _worldRootBinder;


        public Observable<GameplayExitParams> Run(DiContainer gameplayContainer, GameplayEnterParams enterParams)
        {
            GameplayRegistrations.Register(gameplayContainer, enterParams);
            var UIRoot = gameplayContainer.Resolve<UIRootView>();
            var UIScene = Instantiate(_sceneUIRootPrefab);
            UIRoot.AttachSceneUI(UIScene.gameObject);

            var exitSceneSignalSubj = new Subject<Unit>();
            UIScene.Bind(exitSceneSignalSubj);
            _worldRootBinder.Bind(gameplayContainer);

            var mainMenuEnterParams = new MainMenuEnterParams("mainMenuEnterParamsTestString");
            var exitParams = new GameplayExitParams(mainMenuEnterParams);
            var exitToMainMenuSceneSignal = exitSceneSignalSubj.Select(_ => exitParams);

            return exitToMainMenuSceneSignal;
        }
    }
}
