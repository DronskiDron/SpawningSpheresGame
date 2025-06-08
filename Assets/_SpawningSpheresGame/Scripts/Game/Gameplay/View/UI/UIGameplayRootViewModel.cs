using SpawningSpheresGame.Game.Common.MVVM.UI;
using Zenject;

namespace SpawningSpheresGame.Scripts.Game.Gameplay.View.UI
{
    public class UIGameplayRootViewModel : UIRootViewModel
    {
        private readonly DiContainer _container;

        private GameplayUIManager _uIManager;

        public UIGameplayRootViewModel(DiContainer container)
        {
            _container = container;

            var uiManager = container.Resolve<GameplayUIManager>();
            _uIManager = uiManager;
        }

        public void OpenScreenGameplay()
        {
            _uIManager.OpenScreenGameplay();
        }

        public void OpenPlayerControlScreen()
        {
            _uIManager.OpenPlayerControlScreen();
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}