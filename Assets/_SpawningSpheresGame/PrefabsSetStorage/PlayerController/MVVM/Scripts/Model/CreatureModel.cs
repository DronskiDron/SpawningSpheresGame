using System;
using R3;
using SpawningSpheresGame.Game.Common.DataTypes;
using SpawningSpheresGame.Game.Settings.DataStorage;
using SpawningSpheresGame.Game.State.Entities.Creatures;
using Zenject;

namespace PlayerController.MVVM
{
    public abstract class CreatureModel<TCreature, TConfig> : IDisposable, IModel
           where TCreature : ICreatureEntity<CreatureEntityData>
           where TConfig : CreatureConfig
    {
        private readonly CompositeDisposable _subscriptions = new();

        public DiContainer Container { get; }
        public TCreature CreatureEntity { get; }
        public TConfig Config { get; }


        public CreatureModel(TCreature creatureEntity, TConfig config, DiContainer container)
        {
            CreatureEntity = creatureEntity;
            Config = config;
            Container = container;
        }


        public void Dispose()
        {
            _subscriptions.Dispose();
            CreatureEntity.Dispose();
        }


        public void OnDestroyModel()
        {
            Dispose();
        }
    }
}