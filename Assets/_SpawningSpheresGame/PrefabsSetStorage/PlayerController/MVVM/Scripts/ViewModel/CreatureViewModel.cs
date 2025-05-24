using System;
using R3;
using SpawningSpheresGame.Game.Common.DataTypes;
using SpawningSpheresGame.Game.Settings.DataStorage;
using SpawningSpheresGame.Game.State.Entities.Creatures;
using UnityEngine;
using Zenject;

namespace PlayerController.MVVM
{
    public abstract class CreatureViewModel<TCreature, TConfig> : IDisposable, IViewModel, ITickable, IFixedTickable
           where TCreature : ICreatureEntity<CreatureEntityData>
           where TConfig : CreatureConfig
    {
        protected readonly CompositeDisposable Subscriptions = new();

        private readonly Subject<Unit> _tickSubject = new Subject<Unit>();
        private readonly Subject<Unit> _fixedTickSubject = new Subject<Unit>();

        public Observable<Unit> OnTick => _tickSubject;
        public Observable<Unit> OnFixedTick => _fixedTickSubject;

        public ReactiveProperty<Vector3> Position = new();
        public ReactiveProperty<Quaternion> Rotation = new();

        public CreatureInput Input { get; } = new CreatureInput();
        public CreatureModel<TCreature, TConfig> Model { get; private set; }


        public CreatureViewModel(CreatureModel<TCreature, TConfig> model)
        {
            Model = model;
            BindToModel(model);
        }


        public abstract void InitHandlers();


        protected virtual void BindToModel(CreatureModel<TCreature, TConfig> model)
        {
            model.CreatureEntity.TransformState.Position
                .Subscribe(newValue => Position.Value = newValue).AddTo(Subscriptions);
            model.CreatureEntity.TransformState.Rotation
                .Subscribe(newValue => Rotation.Value = newValue).AddTo(Subscriptions);
        }


        public virtual void UpdateTransformState(Vector3 position, Quaternion rotation)
        {
            Model.CreatureEntity.TransformState.Position.Value = position;
            Model.CreatureEntity.TransformState.Rotation.Value = rotation;
        }


        public virtual void Tick()
        {
            _tickSubject.OnNext(Unit.Default);
        }


        public virtual void FixedTick()
        {
            _fixedTickSubject.OnNext(Unit.Default);
        }


        public void Dispose()
        {
            Subscriptions.Dispose();
            Model = null;
        }


        public void OnDestroyViewModel()
        {
            Dispose();
        }
    }
}