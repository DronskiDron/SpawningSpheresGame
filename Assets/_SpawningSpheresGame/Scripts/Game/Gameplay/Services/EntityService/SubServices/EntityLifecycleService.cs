using System;
using SpawningSpheresGame.Game.Common.DataTypes;
using UnityEngine;
using Zenject;

namespace SpawningSpheresGame.Game.Gameplay.Services
{
    public class EntityLifecycleService : IDisposable
    {
        private readonly TickableManager _tickableManager;

        public EntityLifecycleService(DiContainer container)
        {
            _tickableManager = container.Resolve<TickableManager>();
        }

        public void RegisterTickables(MVVMDataStack mvvmStack)
        {
            try
            {
                if (mvvmStack.ViewModel is ITickable tickable)
                {
                    _tickableManager.Add(tickable);
                }

                if (mvvmStack.ViewModel is IFixedTickable fixedTickable)
                {
                    _tickableManager.AddFixed(fixedTickable);
                }

                if (mvvmStack.ViewModel is ILateTickable lateTickable)
                {
                    _tickableManager.AddLate(lateTickable);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error registering tickables: {ex.Message}");
            }
        }

        public void UnregisterTickables(MVVMDataStack mvvmStack)
        {
            try
            {
                if (mvvmStack.ViewModel is ITickable tickable)
                {
                    _tickableManager.Remove(tickable);
                }

                if (mvvmStack.ViewModel is IFixedTickable fixedTickable)
                {
                    _tickableManager.RemoveFixed(fixedTickable);
                }

                if (mvvmStack.ViewModel is ILateTickable lateTickable)
                {
                    _tickableManager.RemoveLate(lateTickable);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error unregistering tickables: {ex.Message}");
            }
        }

        public void DestroyMVVMStack(MVVMDataStack mvvmStack)
        {
            try
            {
                mvvmStack.View.OnDestroyView();
                mvvmStack.ViewModel.OnDestroyViewModel();
                mvvmStack.Model.OnDestroyModel();
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error destroying MVVM stack: {ex.Message}");
            }
        }

        private bool _isDisposed = false;

        public void Dispose()
        {
            if (_isDisposed) return;
            _isDisposed = true;
        }
    }
}