using System;

namespace SpawningSpheresGame.JsonSerializationExample
{
    [Serializable]
    public class CharacterEntityJsonTest : EntityJsonTest
    {
        public int Level { get; set; }
    }
}