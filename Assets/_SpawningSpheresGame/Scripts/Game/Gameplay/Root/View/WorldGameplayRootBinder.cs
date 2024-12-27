using SpawningSpheresGame.Game.Settings;
using SpawningSpheresGame.PrefabPrefabsStorage.RTS_camera.CameraZoom.MVVM_ZoomPreset;
using UnityEngine;
using Zenject;

namespace SpawningSpheresGame.Game.Gameplay.Root.View
{
    public class WorldGameplayRootBinder : MonoBehaviour
    {
        public void Bind(DiContainer container)
        {
            var gameSettings = container.Resolve<ISettingsProvider>().GameSettings;
            var storedPefabsList = gameSettings.MainStorage.PrefabsStorage.PrefabsList;
            foreach (var item in storedPefabsList)
            {
                Debug.Log($"Prefab Id: {item.Id}");
            }
            // var cameraSystem = new ZoomCameraInitializer(container);
            // container.BindInstance(cameraSystem).AsSingle();

        }
    }
}
