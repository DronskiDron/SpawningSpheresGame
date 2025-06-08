using System;
using PlayerController.MVVM.View.BinderComponents;
using R3;
using SpawningSpheresGame.Game.Common.DataTypes;
using SpawningSpheresGame.Game.Settings.DataStorage;
using SpawningSpheresGame.Game.State.Entities.Creatures;
using UnityEngine;

namespace PlayerController.MVVM
{
    public abstract class CreatureBinder<TCreature, TConfig> : MonoBehaviour, IView, IDisposable
        where TCreature : ICreatureEntity<CreatureEntityData>
        where TConfig : CreatureConfig
    {
        protected readonly CompositeDisposable Subscriptions = new();
        [SerializeField] protected Rigidbody Rb;
        [SerializeField] protected GroundChecker _groundCheck;

        protected CreatureViewModel<TCreature, TConfig> ViewModel;
        protected GroundCheckBinderComponent GroundCheckComponent;

        public virtual void Init(CreatureViewModel<TCreature, TConfig> viewModel)
        {
            ViewModel = viewModel;

            GroundCheckComponent = new GroundCheckBinderComponent(_groundCheck, viewModel.Input, viewModel.OnFixedTick);
            GroundCheckComponent.Initialize();

            viewModel.InitHandlers();
        }

        public virtual void Dispose()
        {
            GroundCheckComponent?.Dispose();
            Subscriptions.Dispose();
        }

        private void OnDestroy()
        {
            Dispose();
        }

        public virtual void OnDestroyView()
        {
            Dispose();
        }
    }
}
