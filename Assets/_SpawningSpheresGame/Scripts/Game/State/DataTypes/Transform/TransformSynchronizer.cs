using System;
using R3;
using UnityEngine;

namespace SpawningSpheresGame.Game.State.DataTypes
{
    public class TransformSynchronizer : IDisposable
    {
        private readonly CompositeDisposable _subscriptions = new();
        private readonly Transform _transform;
        private readonly TransformState _transformState;
        private bool _isSynchronizing = false;

        public ReactiveProperty<Vector3> Position { get; }
        public ReactiveProperty<Quaternion> Rotation { get; }
        public ReactiveProperty<Vector3> Scale { get; }

        public TransformSynchronizer(Transform transform, TransformState transformState)
        {
            _transform = transform;
            _transformState = transformState;

            Position = new ReactiveProperty<Vector3>(transform.position);
            Rotation = new ReactiveProperty<Quaternion>(transform.rotation);
            Scale = new ReactiveProperty<Vector3>(transform.localScale);

            Position.Subscribe(newPos =>
            {
                if (!_isSynchronizing)
                {
                    ApplyPosition(newPos);
                }
            }).AddTo(_subscriptions);

            Rotation.Subscribe(newRot =>
            {
                if (!_isSynchronizing)
                {
                    ApplyRotation(newRot);
                }
            }).AddTo(_subscriptions);

            Scale.Subscribe(newScale =>
            {
                if (!_isSynchronizing)
                {
                    ApplyScale(newScale);
                }
            }).AddTo(_subscriptions);

            _transformState.Position.Subscribe(newPos =>
            {
                if (!_isSynchronizing && newPos != Position.Value)
                {
                    _isSynchronizing = true;
                    Position.Value = newPos;
                    _isSynchronizing = false;
                }
            }).AddTo(_subscriptions);

            _transformState.Rotation.Subscribe(newRot =>
            {
                if (!_isSynchronizing && newRot != Rotation.Value)
                {
                    _isSynchronizing = true;
                    Rotation.Value = newRot;
                    _isSynchronizing = false;
                }
            }).AddTo(_subscriptions);

            _transformState.Scale.Subscribe(newScale =>
            {
                if (!_isSynchronizing && newScale != Scale.Value)
                {
                    _isSynchronizing = true;
                    Scale.Value = newScale;
                    _isSynchronizing = false;
                }
            }).AddTo(_subscriptions);
        }

        public void CheckForTransformChanges()
        {
            if (_isSynchronizing) return;

            if (_transform.position != Position.Value)
            {
                _isSynchronizing = true;
                Position.Value = _transform.position;
                _isSynchronizing = false;
            }

            if (_transform.rotation != Rotation.Value)
            {
                _isSynchronizing = true;
                Rotation.Value = _transform.rotation;
                _isSynchronizing = false;
            }

            if (_transform.localScale != Scale.Value)
            {
                _isSynchronizing = true;
                Scale.Value = _transform.localScale;
                _isSynchronizing = false;
            }
        }

        private void ApplyPosition(Vector3 position)
        {
            _isSynchronizing = true;
            _transform.position = position;
            _transformState.Position.Value = position;
            _isSynchronizing = false;
        }

        private void ApplyRotation(Quaternion rotation)
        {
            _isSynchronizing = true;
            _transform.rotation = rotation;
            _transformState.Rotation.Value = rotation;
            _isSynchronizing = false;
        }

        private void ApplyScale(Vector3 scale)
        {
            _isSynchronizing = true;
            _transform.localScale = scale;
            _transformState.Scale.Value = scale;
            _isSynchronizing = false;
        }

        public void Dispose()
        {
            _subscriptions.Dispose();
        }
    }
}