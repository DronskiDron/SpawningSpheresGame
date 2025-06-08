using SpawningSpheresGame.Game.Common.DataTypes.Enums;
using SpawningSpheresGame.Game.GameRoot.RootManagers;
using UnityEngine;

namespace SpawningSpheresGame.Game.Gameplay.Root.View
{
    public class WorldGameplayRootBinder : MonoBehaviour
    {
        [SerializeField] private WorldContainerComponent _worldContainerComponent;

        public WorldContainerComponent WorldContainerComponent => _worldContainerComponent;

        private WorldGameplayRootViewModel _viewModel;

        public void Bind(WorldGameplayRootViewModel viewModel)
        {
            _viewModel = viewModel;
            _viewModel.RestoreGameState();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                _viewModel.DeleteEntity(0);
            }
            if (Input.GetKeyDown(KeyCode.X))
            {
                _viewModel.DeleteEntity(1);
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                _viewModel.DeleteEntity(2);
            }
            if (Input.GetKeyDown(KeyCode.V))
            {
                _viewModel.DeleteEntity(3);
            }


            if (Input.GetKeyDown(KeyCode.R))
            {
                _viewModel.CreateEntity(GameplayEntitiesId.Test, ObjectContainersEnum.CameraContainer);
            }
            if (Input.GetKeyDown(KeyCode.T))
            {
                _viewModel.CreateEntity(GameplayEntitiesId.ZoomRtsCamera, ObjectContainersEnum.CameraContainer);
            }
            if (Input.GetKeyDown(KeyCode.Y))
            {
                _viewModel.CreateEntity(GameplayEntitiesId.MoveRtsCamera, ObjectContainersEnum.CameraContainer);
            }
            if (Input.GetKeyDown(KeyCode.U))
            {
                _viewModel.CreateEntity(GameplayEntitiesId.Player, ObjectContainersEnum.CreatureContainer);
            }
            if (Input.GetKeyDown(KeyCode.O))
            {
                _viewModel.SaveGameState();
            }
        }

        private void OnDestroy()
        {
            _viewModel = null;
        }
    }
}
