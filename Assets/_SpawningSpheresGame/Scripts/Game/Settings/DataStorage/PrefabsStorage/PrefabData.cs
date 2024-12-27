using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpawningSpheresGame.Game.Settings.DataStorage
{
    [Serializable]
    public class PrefabData
    {
        [SerializeField] private string _id;
        [SerializeField] private List<TransformPreset> _transformPresets;

        public string Id { get => _id; }
        public IReadOnlyList<TransformPreset> TransformPresets { get => _transformPresets; }
    }
}