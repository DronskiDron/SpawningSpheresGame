using SpawningSpheresGame.Game.Common.MVVM.UI;
using UnityEngine;
using UnityEngine.UI;

namespace SpawningSpheresGame.Scripts.Game.Gameplay.View.UI.PlayerControlScreen
{
    public class PlayerControlScreenBinder : WindowBinder<PlayerControlScreenViewModel>
    {
        [SerializeField] private Button _btnGoToMenu;

        private void OnEnable()
        {
            _btnGoToMenu.onClick.AddListener(OnGoToMainMenuButtonClicked);
        }

        private void OnDisable()
        {
            _btnGoToMenu.onClick.RemoveListener(OnGoToMainMenuButtonClicked);
        }

        private void OnGoToMainMenuButtonClicked()
        {
            ViewModel.RequestGoToMainMenu();
        }
    }
}