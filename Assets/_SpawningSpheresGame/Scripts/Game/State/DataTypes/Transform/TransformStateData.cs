using System;
using UnityEngine;

namespace SpawningSpheresGame.Game.State.DataTypes
{
    [Serializable]
    public class TransformStateData
    {
        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }
        public Vector3 Scale { get; set; }

        public TransformStateData(Vector3 position = default, Quaternion rotation = default, Vector3 scale = default)
        {
            Position = position;
            Rotation = rotation;
            Scale = scale;
        }

        public TransformStateData Clone()
        {
            return new TransformStateData
            {
                Position = Position,
                Rotation = Rotation,
                Scale = Scale
            };
        }
    }
}