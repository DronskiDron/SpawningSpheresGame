using R3;
using SpawningSpheresGame.Game.Common;
using SpawningSpheresGame.Game.Common.MVVM.UI;
using SpawningSpheresGame.Scripts.Game.Gameplay.View.UI.PlayerControlScreen;
using SpawningSpheresGame.Scripts.Game.Gameplay.View.UI.PopupA;
using SpawningSpheresGame.Scripts.Game.Gameplay.View.UI.PopupB;
using SpawningSpheresGame.Scripts.Game.Gameplay.View.UI.ScreenGameplay;
using Zenject;

namespace SpawningSpheresGame.Scripts.Game.Gameplay.View.UI
{
    public class GameplayUIManager : UIManager
    {
        private readonly Subject<Unit> _exitSceneRequest;

        public GameplayUIManager(DiContainer container) : base(container)
        {
            _exitSceneRequest = container.ResolveId<Subject<Unit>>(AppConstants.EXIT_SCENE_REQUEST_TAG);
        }

        public ScreenGameplayViewModel OpenScreenGameplay()
        {
            var viewModel = new ScreenGameplayViewModel(this, _exitSceneRequest);
            var rootUI = Container.Resolve<UIGameplayRootViewModel>();

            rootUI.OpenScreen(viewModel);

            return viewModel;
        }

        public PlayerControlScreenViewModel OpenPlayerControlScreen()
        {
            var viewModel = new PlayerControlScreenViewModel(this, _exitSceneRequest);
            var rootUI = Container.Resolve<UIGameplayRootViewModel>();

            rootUI.OpenScreen(viewModel);

            return viewModel;
        }

        public PopupAViewModel OpenPopupA()
        {
            var a = new PopupAViewModel();
            var rootUI = Container.Resolve<UIGameplayRootViewModel>();

            rootUI.OpenPopup(a);

            return a;
        }

        public PopupBViewModel OpenPopupB()
        {
            var b = new PopupBViewModel();
            var rootUI = Container.Resolve<UIGameplayRootViewModel>();

            rootUI.OpenPopup(b);

            return b;
        }
    }
}