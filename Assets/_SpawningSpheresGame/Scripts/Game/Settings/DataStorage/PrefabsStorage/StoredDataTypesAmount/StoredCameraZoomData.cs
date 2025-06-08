using System;
using UnityEngine;

namespace SpawningSpheresGame.Game.Settings.DataStorage
{
    [Serializable]
    public class StoredCameraZoomData
    {
        [SerializeField] private float _fieldOfView;
        [SerializeField] private float _orthographicSize;

        public float FieldOfView => _fieldOfView;
        public float OrthographicSize => _orthographicSize;


        public StoredCameraZoomData(float fieldOfView = default, float orthographicSize = default)
        {
            _fieldOfView = fieldOfView;
            _orthographicSize = orthographicSize;
        }
    }
}