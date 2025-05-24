using System;
using R3;
using SpawningSpheresGame.Game.Common.DataTypes;
using SpawningSpheresGame.Game.Settings.DataStorage;
using SpawningSpheresGame.Game.State.Entities.Cameras;
using UnityEngine;

namespace MVVMCameraMovement
{
    public class MoveRtsCameraBinder<TEntity, TConfig> : MonoBehaviour, IView, IDisposable
        where TEntity : IMoveRtsCameraEntity<MoveRtsCameraEntityData>
        where TConfig : MoveRtsCameraConfig
    {
        private readonly CompositeDisposable _subscriptions = new();

        [SerializeField] private Camera _camera;
        [SerializeField] private LayerMask _raycastMask;
        [SerializeField] private LayerMask _groundMask;

        private MoveRtsCameraMoveHandlerBinderComponent _moveHandlerComponent;
        private MoveRtsCameraInputBinderComponent _inputComponent;

        public void Init(MoveRtsCameraViewModel<TEntity, TConfig> viewModel)
        {
            BindToViewModel(viewModel);

            if (viewModel is MoveRtsCameraViewModel<MoveRtsCameraEntity, MoveRtsCameraConfig> moveRtsCameraViewModel)
            {
                _inputComponent = new MoveRtsCameraInputBinderComponent(viewModel.GetApplicationInputController()
                    , moveRtsCameraViewModel, _camera, _raycastMask, _groundMask);

                _moveHandlerComponent = new MoveRtsCameraMoveHandlerBinderComponent(
                    moveRtsCameraViewModel,
                    _camera);

                moveRtsCameraViewModel.InitTransformSynchronizer(_camera.gameObject.transform
                    , moveRtsCameraViewModel.GetEntity().TransformState);

                _inputComponent.Initialize();
                _moveHandlerComponent.Initialize();

                moveRtsCameraViewModel.InitMovementLogicIfEverythingIsReady();
            }
        }

        private void BindToViewModel(MoveRtsCameraViewModel<TEntity, TConfig> viewModel) { }

        public void Dispose()
        {
            _inputComponent?.Dispose();
            _moveHandlerComponent?.Dispose();
            _subscriptions.Dispose();
        }

        private void OnDestroy()
        {
            Dispose();
        }

        public void OnDestroyView()
        {
            Dispose();
        }
    }

    public class MoveRtsCameraBinder : MoveRtsCameraBinder<MoveRtsCameraEntity, MoveRtsCameraConfig> { }
}