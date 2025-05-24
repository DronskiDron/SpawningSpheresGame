using System;

namespace SpawningSpheresGame.Game.State.Entities.Test
{
    [Serializable]
    public class TestEntityData : EntityData
    {
        public string Message { get; set; } = "Test Entity";


        public TestEntityData()
        {
            Type = EntityType.TestEntity;
        }
    }
}