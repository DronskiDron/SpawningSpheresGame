using System;
using UnityEngine;

namespace SpawningSpheresGame.Game.Settings.DataStorage
{
    [Serializable]
    public class TransformPreset
    {
        [SerializeField] private string _presetId;
        [SerializeField] private Vector3 _position;
        [SerializeField] private Quaternion _rotation = Quaternion.identity;
        [SerializeField] private Vector3 _scale = Vector3.one;

        public string presetId => _presetId;
        public Vector3 Position => _position;
        public Quaternion Rotation => _rotation;
        public Vector3 Scale => _scale;


        public TransformPreset(Vector3 position, Quaternion rotation, Vector3 scale, string presetId = "NoId")
        {
            this._position = position;
            this._rotation = rotation;
            this._scale = scale;
            this._presetId = presetId;
        }
    }
}