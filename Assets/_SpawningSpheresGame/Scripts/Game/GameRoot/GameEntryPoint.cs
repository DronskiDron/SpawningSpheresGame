using SpawningSpheresGame.Game.Gameplay.Root;
using SpawningSpheresGame.Game.Gameplay.Root.GameplayParams;
using SpawningSpheresGame.Game.MainMenu.Root.MainMenuParams;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
using R3;
using SpawningSpheresGame.Game.MainMenu.Root;
using SpawningSpheresGame.Game.Settings;
using SpawningSpheresGame.Game.State;
using SpawningSpheresGame.Utils.JsonSerialization;
using SpawningSpheresGame.Game.Settings.Controlls;
using SpawningSpheresGame.Utils.DI;
using SpawningSpheresGame.Game.GameRoot.RootManagers;
using Cysharp.Threading.Tasks;

namespace SpawningSpheresGame.Game.GameRoot
{
    public class GameEntryPoint
    {
        private static GameEntryPoint _instance;
        private UIRootView _UIRoot;
        private DiContainer _rootContainer = new();
        private DiContainer _cachedSceneContainer;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void AutostartGame()
        {
            _instance = new GameEntryPoint();
            JsonProjectSettings.ApplyProjectSerializationSettings();
            _instance.RunGame().Forget();
        }

        public GameEntryPoint()
        {
            var prefabUIRoot = Resources.Load<UIRootView>("UIRoot");
            _UIRoot = Object.Instantiate(prefabUIRoot);
            Object.DontDestroyOnLoad(_UIRoot.gameObject);
            _rootContainer.BindInstance(_UIRoot).AsSingle();

            var settingsProvider = new SettingsProvider();
            _rootContainer.BindInstance<ISettingsProvider>(settingsProvider).AsSingle();

            var gameStateProvider = new PlayerPrefsGameStateProvider();
            gameStateProvider.LoadSettingsState();

            _rootContainer.BindInstance<IGameStateProvider>(gameStateProvider);

            var rootSceneContainersManager = new RootSceneContainersManager(_rootContainer);
            _rootContainer.BindInstance(rootSceneContainersManager);

            InitInput();

            Application.quitting += OnApplicationQuitHandler;
        }

        private async UniTask RunGame()
        {
            await _rootContainer.Resolve<ISettingsProvider>().LoadGameSettings();

#if UNITY_EDITOR
            var sceneName = SceneManager.GetActiveScene().name;

            if (sceneName == Scenes.GAMEPLAY)
            {
                var enterParams = new GameplayEnterParams();
                LoadAndStartGameplay(enterParams).Forget();
                return;
            }

            if (sceneName == Scenes.MAIN_MENU)
            {
                LoadAndStartMainMenu().Forget();
                return;
            }

            if (sceneName != Scenes.BOOT)
            {
                return;
            }
#endif
            LoadAndStartMainMenu().Forget();
        }

        private async UniTaskVoid LoadAndStartGameplay(GameplayEnterParams enterParams)
        {
            _UIRoot.ShowLoadingScreen();

            if (_cachedSceneContainer != null)
            {
                _cachedSceneContainer.DisposeAndFlush();
                _cachedSceneContainer = null;
            }

            await LoadScene(Scenes.BOOT);
            await LoadScene(Scenes.GAMEPLAY);

            await UniTask.Delay(1000);

            var isGameStateLoaded = false;
            _rootContainer.Resolve<IGameStateProvider>().LoadGameState().Subscribe(_ => isGameStateLoaded = true);
            await UniTask.WaitUntil(() => isGameStateLoaded);

            var sceneEntryPoint = Object.FindObjectOfType<GameplayEntryPoint>();
            var gameplayContainer = _cachedSceneContainer = new DiContainer(_rootContainer);
            sceneEntryPoint.Run(gameplayContainer, enterParams).Subscribe(gameplayExitParams =>
            {
                LoadAndStartMainMenu(gameplayExitParams.MainMenuEnterParams).Forget();
            });

            _UIRoot.HideLoadingScreen();
        }

        private async UniTaskVoid LoadAndStartMainMenu(MainMenuEnterParams enterParams = null)
        {
            _UIRoot.ShowLoadingScreen();

            if (_cachedSceneContainer != null)
            {
                _cachedSceneContainer.DisposeAndFlush();
                _cachedSceneContainer = null;
            }

            await LoadScene(Scenes.BOOT);
            await LoadScene(Scenes.MAIN_MENU);

            await UniTask.Delay(1000);

            var sceneEntryPoint = Object.FindObjectOfType<MainMenuEntryPoint>();
            var mainMenuContainer = _cachedSceneContainer = new DiContainer(_rootContainer);
            sceneEntryPoint.Run(mainMenuContainer, enterParams).Subscribe(mainMenuExitParams =>
            {
                var targetSceneName = mainMenuExitParams.TargetSceneEnterParams.SceneName;

                if (targetSceneName == Scenes.GAMEPLAY)
                {
                    LoadAndStartGameplay(mainMenuExitParams.TargetSceneEnterParams.
                    As<GameplayEnterParams>()).Forget();
                }
            });

            _UIRoot.HideLoadingScreen();
        }

        private async UniTask LoadScene(string sceneName)
        {
            await SceneManager.LoadSceneAsync(sceneName);
        }

        private void InitInput()
        {
            var inputActions = new ApplicationInputController();
            _rootContainer.BindInstance(inputActions).AsSingle();

            var inputManager = new InputSchemeManager(inputActions.asset);
            inputManager.AutoDetectScheme();
            // inputManager.SetControlScheme(ControlSchemes.Mobile);

            inputActions.Enable();
        }

        private void OnApplicationQuitHandler()
        {
            if (_cachedSceneContainer != null)
            {
                _cachedSceneContainer.DisposeAndFlush();
                _cachedSceneContainer = null;
            }

            _rootContainer.DisposeAndFlush();
        }
    }
}
