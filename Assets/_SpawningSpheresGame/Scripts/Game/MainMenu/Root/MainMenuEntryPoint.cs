using R3;
using SpawningSpheresGame.Game.Gameplay.Root.GameplayParams;
using SpawningSpheresGame.Game.GameRoot;
using SpawningSpheresGame.Game.MainMenu.Root.MainMenuParams;
using SpawningSpheresGame.Game.MainMenu.Root.View;
using UnityEngine;
using Zenject;

namespace SpawningSpheresGame.Game.MainMenu.Root
{
    public class MainMenuEntryPoint : MonoBehaviour
    {
        [SerializeField] private UIMainMenuRootBinder _sceneUIRootPrefab;


        public Observable<MainMenuExitParams> Run(DiContainer mainMenuContainer, MainMenuEnterParams enterParams)
        {
            MainMenuRegistrations.Register(mainMenuContainer, enterParams);
            var UIRoot = mainMenuContainer.Resolve<UIRootView>();
            var UIScene = Instantiate(_sceneUIRootPrefab);
            UIRoot.AttachSceneUI(UIScene.gameObject);

            var exitSceneSignalSubj = new Subject<Unit>();
            UIScene.Bind(exitSceneSignalSubj);

            var gameplayEnterParams = new GameplayEnterParams();
            var mainMenuExitParams = new MainMenuExitParams(gameplayEnterParams);
            var exitToGameplaySceneSignal = exitSceneSignalSubj.Select(_ => mainMenuExitParams);

            return exitToGameplaySceneSignal;
        }
    }
}
