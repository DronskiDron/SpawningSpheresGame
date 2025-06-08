using System.Collections.Generic;

namespace SpawningSpheresGame.JsonSerializationExample
{
    public class GameStateJsonTest
    {
        public int LastId { get; set; }
        public List<EntityJsonTest> Entities { get; set; }


        public int GetNewId()
        {
            return LastId++;
        }
    }
}