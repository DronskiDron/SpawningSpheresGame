using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpawningSpheresGame.Game.Settings.DataStorage
{
    [Serializable]
    public class AdvancedPlayerConfig : PlayerConfig
    {
        [Header("AdvancedPlayer Specific Settings")]
        [SerializeField] private float _specialPower = 20;
        [SerializeField] private int _extraLives = 10;
        [SerializeField] private List<string> _abilities = new List<string> { "SuperJump", "DoubleSpeed" };

        public int ExtraLives => _extraLives;
        public float SpecialPower => _specialPower;
        public List<string> Abilities => _abilities;
    }
}