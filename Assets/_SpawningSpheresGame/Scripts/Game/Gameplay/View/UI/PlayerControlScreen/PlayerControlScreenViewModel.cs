using R3;
using SpawningSpheresGame.Game.Common.MVVM.UI;

namespace SpawningSpheresGame.Scripts.Game.Gameplay.View.UI.PlayerControlScreen
{
    public class PlayerControlScreenViewModel : WindowViewModel
    {
        private readonly GameplayUIManager _uiManager;
        private readonly Subject<Unit> _exitSceneRequest;
        public override string Id => "PlayerControlScreen";

        public PlayerControlScreenViewModel(GameplayUIManager uiManager, Subject<Unit> exitSceneRequest)
        {
            _uiManager = uiManager;
            _exitSceneRequest = exitSceneRequest;
        }

        public void RequestGoToMainMenu()
        {
            _exitSceneRequest.OnNext(Unit.Default);
        }
    }
}