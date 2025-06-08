using System;
using ObservableCollections;
using R3;

namespace SpawningSpheresGame.Game.State.Entities.Creatures.Player
{
    public abstract class AdvancedPlayerEntity<TData> : PlayerEntity<TData>, IAdvancedPlayerEntity<TData>
    where TData : AdvancedPlayerEntityData
    {
        public ReactiveProperty<float> SpecialPower { get; }
        public ReactiveProperty<int> ExtraLives { get; }
        public ObservableList<string> Abilities { get; }

        private IDisposable _addSubscription;
        private IDisposable _removeSubscription;

        protected AdvancedPlayerEntity(TData data) : base(data)
        {
            SpecialPower = new ReactiveProperty<float>(data.SpecialPower);
            SpecialPower.Skip(1).Subscribe(value => data.SpecialPower = value);

            ExtraLives = new ReactiveProperty<int>(data.ExtraLives);
            ExtraLives.Skip(1).Subscribe(value => data.ExtraLives = value);

            Abilities = new ObservableList<string>(data.Abilities);
            _addSubscription = Abilities.ObserveAdd().Subscribe(e => data.Abilities.Add(e.Value));
            _removeSubscription = Abilities.ObserveRemove().Subscribe(e => data.Abilities.Remove(e.Value));
        }

        public override void Dispose()
        {
            base.Dispose();
            SpecialPower.Dispose();
            ExtraLives.Dispose();

            _addSubscription.Dispose();
            _removeSubscription.Dispose();
        }
    }

    public class AdvancedPlayerEntity : AdvancedPlayerEntity<AdvancedPlayerEntityData>, IAdvancedPlayerEntity
    {
        public AdvancedPlayerEntity(AdvancedPlayerEntityData data) : base(data) { }

        EntityData IEntity<EntityData>.Origin => Origin;

        CreatureEntityData IEntity<CreatureEntityData>.Origin => Origin;

        PlayerEntityData IEntity<PlayerEntityData>.Origin => Origin;
    }
}