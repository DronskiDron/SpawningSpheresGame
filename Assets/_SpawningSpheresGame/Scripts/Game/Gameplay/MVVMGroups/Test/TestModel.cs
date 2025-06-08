using System;
using R3;
using SpawningSpheresGame.Game.Common.DataTypes;
using SpawningSpheresGame.Game.Settings.DataStorage;
using SpawningSpheresGame.Game.State.Entities.Test;
using UnityEngine;
using Zenject;

namespace Gameplay.MVVMGroups.Test
{
    public class TestModel : IDisposable, IModel
    {
        private readonly CompositeDisposable _subscriptions = new();

        public DiContainer Container { get; }
        public ITestEntity TestEntity { get; }
        public TestConfig Config { get; }


        public TestModel(TestEntity testEntity, TestConfig config, DiContainer container)
        {
            TestEntity = testEntity;
            Config = config;
            Container = container;

            if (config.EnableDebugLogs)
                Debug.Log($"TestModel initialized with entity #{testEntity.UniqueId}");
        }


        public void Dispose()
        {
            if (Config.EnableDebugLogs)
                Debug.Log("TestModel disposing");

            _subscriptions.Dispose();
            TestEntity.Dispose();
        }


        public void OnDestroyModel()
        {
            Dispose();
        }
    }
}