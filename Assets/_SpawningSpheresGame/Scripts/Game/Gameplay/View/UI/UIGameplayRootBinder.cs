using SpawningSpheresGame.Game.Common.MVVM.UI;
using SpawningSpheresGame.Scripts.Game.Gameplay.View.UI;
using UnityEngine;

namespace SpawningSpheresGame.Game.Gameplay.Root.View
{
    public class UIGameplayRootBinder : UIRootBinder
    {
        private UIGameplayRootViewModel _viewModel;

        protected override void OnBind(UIRootViewModel rootViewModel)
        {
            if (rootViewModel is UIGameplayRootViewModel uiGameplayRootViewModel)
            {
                _viewModel = uiGameplayRootViewModel;
            }
        }

        private void Update()
        {
            if (_viewModel == null) return;

            if (Input.GetKeyDown(KeyCode.G))
            {
                _viewModel?.OpenScreenGameplay();
            }
            if (Input.GetKeyDown(KeyCode.H))
            {
                _viewModel?.OpenPlayerControlScreen();
            }
        }

        protected override void OnDestroy()
        {
            _viewModel = null;
            base.OnDestroy();
        }
    }
}