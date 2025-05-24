using System;
using SpawningSpheresGame.Game.State.DataTypes;
using UnityEngine;

namespace SpawningSpheresGame.JsonSerializationExample
{
    public enum EntityTypeJsonTest
    {
        Character,
        Door,
        PickubleItem
    }


    [Serializable]
    public class EntityJsonTest
    {
        public int Id { get; set; }
        public EntityTypeJsonTest Type { get; set; }
        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }
        public TransformStateData TransformStateData = new();
    }
}