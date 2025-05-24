using System;
using R3;
using SpawningSpheresGame.Game.Common.DataTypes;
using SpawningSpheresGame.Game.Settings.DataStorage;
using SpawningSpheresGame.Game.State.DataTypes;
using SpawningSpheresGame.Game.State.Entities.Cameras;
using UnityEngine;
using Zenject;

namespace MVVMCameraMovement
{
    public class MoveRtsCameraViewModel<TEntity, TConfig> : IDisposable, IViewModel, ITickable
        where TEntity : IMoveRtsCameraEntity<MoveRtsCameraEntityData>
        where TConfig : MoveRtsCameraConfig
    {
        private readonly CompositeDisposable _subscriptions = new();
        private readonly Subject<Unit> _tickSubject = new Subject<Unit>();

        public Observable<Unit> OnTick => _tickSubject;
        public MoveRtsCameraPreset CameraPreset { get; private set; }
        public TransformSynchronizer TransformSynchronizer { get; private set; }
        public MoveRtsCameraMovementViewModelComponent MoveRtsCameraMovementComponent { get; private set; }

        private MoveRtsCameraModel<TEntity, TConfig> _model;

        public MoveRtsCameraViewModel(MoveRtsCameraModel<TEntity, TConfig> model)
        {
            _model = model;
            BindToModel(model);

            if (this is MoveRtsCameraViewModel<MoveRtsCameraEntity, MoveRtsCameraConfig> viewModel)
            {
                MoveRtsCameraMovementComponent = new MoveRtsCameraMovementViewModelComponent(viewModel);
            }
        }

        public void BindToModel(MoveRtsCameraModel<TEntity, TConfig> model) { }

        public ApplicationInputController GetApplicationInputController()
        {
            var applicationInputController = _model.Container.Resolve<ApplicationInputController>();
            return applicationInputController;
        }

        public TConfig GetConfig()
        {
            return _model.Config;
        }

        public TEntity GetEntity()
        {
            return _model.Entity;
        }

        public void SetCameraPreset(MoveRtsCameraPreset cameraPreset)
        {
            CameraPreset = cameraPreset;
        }

        public void InitMoveHandler(IRtsCameraMovementHandler moveHandler)
        {
            MoveRtsCameraMovementComponent.InitializeMovementHandler(moveHandler);
        }

        public void InitTransformSynchronizer(Transform transform, TransformState transformState)
        {
            TransformSynchronizer = new(transform, transformState);
        }

        public void InitMovementLogicIfEverythingIsReady()
        {
            MoveRtsCameraMovementComponent.Initialize();
        }

        public void Tick()
        {
            _tickSubject.OnNext(Unit.Default);
        }

        public void Dispose()
        {
            MoveRtsCameraMovementComponent.Dispose();
            TransformSynchronizer.Dispose();
            _subscriptions.Dispose();
            _tickSubject.Dispose();
            _model = null;
        }

        public void OnDestroyViewModel()
        {
            Dispose();
        }
    }

    public class MoveRtsCameraViewModel : MoveRtsCameraViewModel<MoveRtsCameraEntity, MoveRtsCameraConfig>
    {
        public MoveRtsCameraViewModel(MoveRtsCameraModel<MoveRtsCameraEntity, MoveRtsCameraConfig> model)
            : base(model) { }
    }
}