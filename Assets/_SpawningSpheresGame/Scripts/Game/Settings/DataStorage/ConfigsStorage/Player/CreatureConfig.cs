using System;
using PlayerController.Classic;
using UnityEngine;

namespace SpawningSpheresGame.Game.Settings.DataStorage
{
    [Serializable]
    public class CreatureConfig : BaseConfig
    {
        [SerializeField] private CharacterSettings _characterSettings = new();

        public CharacterSettings CharacterSettings => _characterSettings;
    }
}