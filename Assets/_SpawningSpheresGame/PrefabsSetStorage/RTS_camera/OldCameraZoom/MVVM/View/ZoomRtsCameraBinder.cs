using System;
using MVVMZoomCamera.View.BinderComponents;
using R3;
using SpawningSpheresGame.Game.Common.DataTypes;
using SpawningSpheresGame.Game.Settings.DataStorage;
using SpawningSpheresGame.Game.State.Entities.Cameras;
using UnityEngine;

namespace MVVMZoomCamera
{
    public class ZoomRtsCameraBinder<TEntity, TConfig> : MonoBehaviour, IView, IDisposable
        where TEntity : IZoomRtsCameraEntity<ZoomRtsCameraEntityData>
        where TConfig : ZoomRtsCameraConfig
    {
        private readonly CompositeDisposable _subscriptions = new();

        [SerializeField] private Camera _camera;

        private ZoomRtsCameraInputComponent _inputComponent;
        private ZoomRtsCameraZoomHandlerComponent _zoomHandlerComponent;

        public void Init(ZoomRtsCameraViewModel<TEntity, TConfig> viewModel)
        {
            if (viewModel is ZoomRtsCameraViewModel<ZoomRtsCameraEntity, ZoomRtsCameraConfig> zoomRtsCameraViewModel)
            {
                _inputComponent = new ZoomRtsCameraInputComponent(viewModel.GetApplicationInputController(), zoomRtsCameraViewModel);
                _zoomHandlerComponent = new ZoomRtsCameraZoomHandlerComponent(zoomRtsCameraViewModel, _camera);

                _inputComponent.Initialize();
                _zoomHandlerComponent.Initialize();
            }

            BindToViewModel(viewModel);
        }

        private void BindToViewModel(ZoomRtsCameraViewModel<TEntity, TConfig> viewModel)
        {
            viewModel.OrthographicSize.Subscribe(newValue => _camera.orthographicSize = newValue).AddTo(_subscriptions);
            viewModel.FieldOfView.Subscribe(newValue => _camera.fieldOfView = newValue).AddTo(_subscriptions);
        }

        public void Dispose()
        {
            _inputComponent?.Dispose();
            _zoomHandlerComponent?.Dispose();
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

    public class ZoomRtsCameraBinder : ZoomRtsCameraBinder<ZoomRtsCameraEntity, ZoomRtsCameraConfig> { }
}