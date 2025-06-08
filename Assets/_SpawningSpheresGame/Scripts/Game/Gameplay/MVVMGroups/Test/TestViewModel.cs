using System;
using R3;
using SpawningSpheresGame.Game.Common.DataTypes;
using UnityEngine;
using Zenject;

namespace Gameplay.MVVMGroups.Test
{
    public class TestViewModel : IDisposable, IViewModel, ITickable, IFixedTickable
    {
        protected readonly CompositeDisposable Subscriptions = new();

        private readonly Subject<Unit> _tickSubject = new Subject<Unit>();
        private readonly Subject<Unit> _fixedTickSubject = new Subject<Unit>();

        public Observable<Unit> OnTick => _tickSubject;
        public Observable<Unit> OnFixedTick => _fixedTickSubject;

        public ReactiveProperty<string> Message = new();
        public TestModel Model { get; private set; }


        public TestViewModel(TestModel model)
        {
            Model = model;
            BindToModel(model);

            if (model.Config.EnableDebugLogs)
                Debug.Log("TestViewModel initialized");
        }


        public void InitHandlers()
        {
            if (Model.Config.EnableDebugLogs)
                Debug.Log("TestViewModel handlers initialized");
        }


        protected virtual void BindToModel(TestModel model)
        {
            model.TestEntity.Message
                .Subscribe(newValue => Message.Value = newValue)
                .AddTo(Subscriptions);
        }


        public void LogMessage()
        {
            // Debug.Log($"TestViewModel message: {Message.Value}");
        }


        public void SetMessage(string message)
        {
            Model.TestEntity.Message.Value = message;
        }


        public virtual void Tick()
        {
            _tickSubject.OnNext(Unit.Default);
            Debug.Log($"Test entity work TempId: {Model.TestEntity.TempId}");
        }


        public virtual void FixedTick()
        {
            _fixedTickSubject.OnNext(Unit.Default);
        }


        public void Dispose()
        {
            if (Model.Config.EnableDebugLogs)
                Debug.Log("TestViewModel disposing");

            Subscriptions.Dispose();
            Model = null;
        }


        public void OnDestroyViewModel()
        {
            Dispose();
        }
    }
}