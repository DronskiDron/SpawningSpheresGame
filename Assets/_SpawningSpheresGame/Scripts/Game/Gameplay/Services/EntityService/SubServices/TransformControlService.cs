using System;
using System.Collections.Generic;
using System.Linq;
using R3;
using SpawningSpheresGame.Game.State.Entities;
using SpawningSpheresGame.Utils.GameplayUtils;
using UnityEngine;
using Zenject;

namespace SpawningSpheresGame.Game.Gameplay.Services
{
    public class TransformControlService : IDisposable
    {
        private readonly CompositeDisposable _subscriptions = new();
        private readonly Dictionary<int, IDisposable> _subscriptionsMap = new();
        private readonly TickableProperties _tickableProperties = new();
        private readonly TickableManager _tickManager;

        public TransformControlService(DiContainer container)
        {
            _tickManager = container.Resolve<TickableManager>();
            RegisterTickables(_tickableProperties);
        }

        public void SubscribeEntityToGoTransform(Transform reference, IEntity<EntityData> entity)
        {
            UnsubscribeEntityFromGoTransform(entity.TempId.Value);

            if (!entity.ControlsTransform.Value)
            {
                var updateSubscription = _tickableProperties.OnFixedTick
                .Subscribe(_ => RenewTransform(reference, entity));

                var controlsSubscription = entity.ControlsTransform.Skip(1).Subscribe(controls =>
                {
                    UnsubscribeEntityFromGoTransform(entity.TempId.Value);
                    SubscribeEntityToGoTransform(reference, entity);
                });

                var combinedSubscription = new CompositeDisposable(updateSubscription, controlsSubscription);

                combinedSubscription.AddTo(_subscriptions);

                _subscriptionsMap[entity.TempId.Value] = combinedSubscription;
            }
        }

        public void UnsubscribeEntityFromGoTransform(int tempId)
        {
            if (_subscriptionsMap.TryGetValue(tempId, out var subscription))
            {
                subscription.Dispose();
                _subscriptionsMap.Remove(tempId);
            }
        }

        private void RenewTransform(Transform reference, IEntity<EntityData> entity)
        {
            var transformState = entity.TransformState;
            transformState.Position.Value = reference.position;
            transformState.Rotation.Value = reference.rotation;
            transformState.Scale.Value = reference.lossyScale;
        }

        private void RegisterTickables(TickableProperties tickableProperties)
        {
            _tickManager.Add(tickableProperties);
            _tickManager.AddFixed(tickableProperties);
            _tickManager.AddLate(tickableProperties);
        }

        private void UnRegisterTickables(TickableProperties tickableProperties)
        {
            _tickManager.Remove(tickableProperties);
            _tickManager.RemoveFixed(tickableProperties);
            _tickManager.RemoveLate(tickableProperties);
        }

        private bool _isDisposed = false;

        public void Dispose()
        {
            if (_isDisposed) return;
            _isDisposed = true;

            _subscriptions.Dispose();

            foreach (var subscription in _subscriptionsMap.Values)
            {
                subscription.Dispose();
            }
            _subscriptionsMap.Clear();

            UnRegisterTickables(_tickableProperties);
        }
    }
}