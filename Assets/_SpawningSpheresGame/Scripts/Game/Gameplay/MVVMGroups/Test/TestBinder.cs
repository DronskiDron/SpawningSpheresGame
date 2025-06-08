using System;
using R3;
using SpawningSpheresGame.Game.Common.DataTypes;
using UnityEngine;

namespace Gameplay.MVVMGroups.Test
{
    public class TestBinder : MonoBehaviour, IView, IDisposable
    {
        protected readonly CompositeDisposable Subscriptions = new();

        protected TestViewModel ViewModel;


        public virtual void Init(TestViewModel viewModel)
        {
            ViewModel = viewModel;
            // Debug.Log("TestBinder initialized");

            BindToViewModel(viewModel);
            viewModel.InitHandlers();

            viewModel.LogMessage();
        }


        protected virtual void BindToViewModel(TestViewModel viewModel)
        {
            viewModel.Message.Subscribe(newMessage =>
            {
                // Debug.Log($"TestBinder received message update: {newMessage}");
            }).AddTo(Subscriptions);

            viewModel.OnTick.Subscribe(_ => OnTick()).AddTo(Subscriptions);
            viewModel.OnFixedTick.Subscribe(_ => OnFixedTick()).AddTo(Subscriptions);
        }


        protected virtual void OnTick() { }


        protected virtual void OnFixedTick() { }


        public void Dispose()
        {
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