using R3;

namespace SpawningSpheresGame.Game.State.Entities.Test
{
    public interface ITestEntity<out TData> : IEntity<TData>
    where TData : TestEntityData
    {
        ReactiveProperty<string> Message { get; }
    }

    public interface ITestEntity : IEntity, ITestEntity<TestEntityData> { }
}