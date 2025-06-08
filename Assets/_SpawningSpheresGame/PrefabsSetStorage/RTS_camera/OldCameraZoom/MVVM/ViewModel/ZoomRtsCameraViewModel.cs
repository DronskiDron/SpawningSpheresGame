using System;
using R3;
using SpawningSpheresGame.Game.Common.DataTypes;
using SpawningSpheresGame.Game.Settings.DataStorage;
using SpawningSpheresGame.Game.State.Entities.Cameras;
using Zenject;

namespace MVVMZoomCamera
{
    public class ZoomRtsCameraViewModel<TEntity, TConfig> : IDisposable, IViewModel, ITickable
        where TEntity : IZoomRtsCameraEntity<ZoomRtsCameraEntityData>
        where TConfig : ZoomRtsCameraConfig
    {
        private readonly CompositeDisposable _subscriptions = new();

        public readonly ReactiveProperty<float> FieldOfView = new();
        public readonly ReactiveProperty<float> OrthographicSize = new();

        public ZoomRtsCameraModel<TEntity, TConfig> Model { get; private set; }
        public ZoomProperties ZoomProperties { get; private set; }

        private IZoomHandler _zoomHandler;
        private float _inputDelta;

        public ZoomRtsCameraViewModel(ZoomRtsCameraModel<TEntity, TConfig> model)
        {
            Model = model;
            BindToModel(model);
        }

        public ApplicationInputController GetApplicationInputController()
        {
            var applicationInputController = Model.Container.Resolve<ApplicationInputController>();
            return applicationInputController;
        }

        public TEntity GetEntity()
        {
            return Model.Entity;
        }

        public void BindToModel(ZoomRtsCameraModel<TEntity, TConfig> model)
        {
            model.Entity.FieldOfView
                 .Subscribe(newValue => FieldOfView.Value = newValue).AddTo(_subscriptions);
            model.Entity.OrthographicSize
                 .Subscribe(newValue => OrthographicSize.Value = newValue).AddTo(_subscriptions);
        }

        public void InitializeZoomHandler(IZoomHandler zoomHandler)
        {
            _zoomHandler = zoomHandler;
        }

        public ZoomProperties GetAndInitZoomProperties(CameraPresetId presetId)
        {
            ZoomProperties = Model.Config.GetCameraPresetById(presetId).ZoomProperties;

            return ZoomProperties;
        }

        public void ReadInputDelta(float delta)
        {
            _inputDelta = delta * ZoomProperties?.VerticalMultiplayer ?? 1f;
        }

        public void Tick()
        {
            _zoomHandler?.Zoom(_inputDelta);
        }

        public void Dispose()
        {
            _subscriptions.Dispose();
            _zoomHandler = null;
            Model = null;
        }

        public void OnDestroyViewModel()
        {
            Dispose();
        }
    }

    public class ZoomRtsCameraViewModel : ZoomRtsCameraViewModel<ZoomRtsCameraEntity, ZoomRtsCameraConfig>
    {
        public ZoomRtsCameraViewModel(ZoomRtsCameraModel<ZoomRtsCameraEntity, ZoomRtsCameraConfig> model)
            : base(model) { }
    }
}