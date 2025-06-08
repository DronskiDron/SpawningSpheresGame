using System;
using UnityEngine;

namespace SpawningSpheresGame.Game.GameRoot.RootManagers
{
    [Serializable]
    public class ObjectsContainerData
    {
        [SerializeField] private ObjectContainersEnum _containerName;
        [SerializeField] private Transform _containerTransform;

        public ObjectContainersEnum ContainerName => _containerName;
        public Transform ContainerTransform => _containerTransform;
    }
}