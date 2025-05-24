using R3;

namespace SpawningSpheresGame.Game.State.Entities.Test
{
    public class TestEntity<TData> : Entity<TData>, ITestEntity<TData>
    where TData : TestEntityData
    {
        public ReactiveProperty<string> Message { get; }

        public TestEntity(TData data) : base(data)
        {
            Message = new ReactiveProperty<string>(data.Message);

            Message.Skip(1).Subscribe(value =>
            {
                data.Message = value;
            });
        }
    }

    public class TestEntity : TestEntity<TestEntityData>, ITestEntity
    {
        public TestEntity(TestEntityData data) : base(data) { }

        EntityData IEntity<EntityData>.Origin => Origin;
    }
}