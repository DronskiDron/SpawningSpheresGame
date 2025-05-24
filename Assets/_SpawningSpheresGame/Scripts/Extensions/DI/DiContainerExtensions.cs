using System;
using SpawningSpheresGame.Game.Gameplay.Services;
using SpawningSpheresGame.Game.GameRoot.RootManagers;
using UnityEngine;
using Zenject;

namespace SpawningSpheresGame.Utils.DI
{
    public static class DiContainerExtensions
    {
        public static void DisposeAndFlush(this DiContainer container)
        {
            DisposeIfExists<GameplayEntityService>(container);
            DisposeIfExists<RestoreGameStateService>(container);

            DisposeIfExists<PrefabInstanceIdService>(container);
            DisposeIfExists<TransformControlService>(container);
            DisposeIfExists<PrefabMatchingService>(container);
            DisposeIfExists<EntityStorageService>(container);
            DisposeIfExists<EntityStateService>(container);
            DisposeIfExists<EntityLifecycleService>(container);
            DisposeIfExists<EntityFactoryService>(container);
            DisposeIfExists<RootSceneContainersManager>(container);

            container.FlushBindings();
        }

        private static void DisposeIfExists<T>(DiContainer container) where T : class, IDisposable
        {
            var service = container.TryResolve<T>();
            if (service != null)
            {
                try
                {
                    service.Dispose();
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Error disposing {typeof(T).Name}: {ex.Message}");
                }
            }
        }
    }
}