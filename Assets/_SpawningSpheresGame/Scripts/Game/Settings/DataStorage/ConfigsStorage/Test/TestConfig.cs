using System;
using UnityEngine;

namespace SpawningSpheresGame.Game.Settings.DataStorage
{
    [Serializable]
    public class TestConfig : BaseConfig
    {
        [SerializeField] private bool _enableDebugLogs = true;

        public bool EnableDebugLogs => _enableDebugLogs;
    }
}