using System;
using UnityEngine;

namespace SpawningSpheresGame.Game.Settings.DataStorage
{
    [Serializable]
    public class TransformPreset
    {
        public const string NO_ID_KEY = nameof(NO_ID_KEY);

        [SerializeField] private string _presetId;
        [SerializeField] private Vector3 _position;
        [SerializeField] private Quaternion _rotation = Quaternion.identity;
        [SerializeField] private Vector3 _scale = Vector3.one;

        public string PresetId => _presetId;
        public Vector3 Position => _position;
        public Quaternion Rotation => _rotation;
        public Vector3 Scale => _scale;


        public TransformPreset(Vector3 position = default, Quaternion rotation = default
                               , Vector3 scale = default, string presetId = NO_ID_KEY)
        {
            _position = position;
            _rotation = rotation;
            _scale = scale;
            _presetId = presetId;
        }
    }
}