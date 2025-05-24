using System;
using UnityEngine;

namespace SpawningSpheresGame.Game.Settings.DataStorage
{
    [Serializable]
    public abstract class BaseConfig : IConfig
    {
        [SerializeField] protected string _configName = "Unnamed Config";
        [SerializeField] protected string _description;

        public string ConfigName => _configName;
        public string Description => _description;
    }
}