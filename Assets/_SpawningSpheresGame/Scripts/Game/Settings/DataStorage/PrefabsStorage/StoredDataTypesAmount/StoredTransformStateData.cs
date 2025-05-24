using System;
using UnityEngine;

namespace SpawningSpheresGame.Game.Settings.DataStorage
{
    [Serializable]
    public class StoredTransformStateData
    {
        [SerializeField] private Vector3 _position;
        [SerializeField] private Quaternion _rotation;
        [SerializeField] private Vector3 _scale;

        public Vector3 Position => _position;
        public Quaternion Rotation => _rotation;
        public Vector3 Scale => _scale;


        public StoredTransformStateData(Vector3 position = default, Quaternion rotation = default, Vector3 scale = default)
        {
            _position = position;
            _rotation = rotation;
            _scale = scale;
        }
    }
}