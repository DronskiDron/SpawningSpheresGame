using R3;
using System;
using UnityEngine;

namespace SpawningSpheresGame.Game.State.DataTypes
{
    [Serializable]
    public class TransformState : IDisposable
    {
        private readonly CompositeDisposable _subscriptions = new();

        public ReactiveProperty<Vector3> Position { get; }
        public ReactiveProperty<Quaternion> Rotation { get; }
        public ReactiveProperty<Vector3> Scale { get; }

        public TransformState(TransformStateData transformStateData)
        {
            Position = new ReactiveProperty<Vector3>(transformStateData.Position);
            Rotation = new ReactiveProperty<Quaternion>(transformStateData.Rotation);
            Scale = new ReactiveProperty<Vector3>(transformStateData.Scale);

            Position.Skip(1).Subscribe(value => transformStateData.Position = value).AddTo(_subscriptions);
            Rotation.Skip(1).Subscribe(value => transformStateData.Rotation = value).AddTo(_subscriptions);
            Scale.Skip(1).Subscribe(value => transformStateData.Scale = value).AddTo(_subscriptions);
        }

        public void Dispose()
        {
            _subscriptions.Dispose();
        }
    }
}