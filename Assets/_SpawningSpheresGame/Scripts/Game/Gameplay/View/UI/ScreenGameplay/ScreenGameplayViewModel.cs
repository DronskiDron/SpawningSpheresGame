using R3;
using SpawningSpheresGame.Game.Common.MVVM.UI;

namespace SpawningSpheresGame.Scripts.Game.Gameplay.View.UI.ScreenGameplay
{
    public class ScreenGameplayViewModel : WindowViewModel
    {
        private readonly GameplayUIManager _uiManager;
        private readonly Subject<Unit> _exitSceneRequest;
        public override string Id => "ScreenGameplay";

        public ScreenGameplayViewModel(GameplayUIManager uiManager, Subject<Unit> exitSceneRequest)
        {
            _uiManager = uiManager;
            _exitSceneRequest = exitSceneRequest;
        }

        public void RequestOpenPopupA()
        {
            _uiManager.OpenPopupA();
        }

        public void RequestOpenPopupB()
        {
            _uiManager.OpenPopupB();
        }

        public void RequestGoToMainMenu()
        {
            _exitSceneRequest.OnNext(Unit.Default);
        }
    }
}