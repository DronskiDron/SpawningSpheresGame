using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SpawningSpheresGame.Game.GameRoot.RootManagers
{
    [Serializable]
    public class ObjectContainers
    {
        [SerializeField] private List<ObjectsContainerData> _conainersList = new();

        public Transform GetContainerByName(ObjectContainersEnum name)
        {
            if (_conainersList == null)
            {
                Debug.LogError($"Containers list is null when trying to get container '{name}'");
                return null;
            }

            var container = _conainersList.FirstOrDefault(c => c.ContainerName == name);

            if (container == null)
            {
                Debug.LogWarning($"Container with name '{name}' not found");
                return null;
            }

            if (container.ContainerTransform == null)
            {
                Debug.LogError($"Container '{name}' has a null transform reference");
                return null;
            }

            return container.ContainerTransform;
        }
    }
}