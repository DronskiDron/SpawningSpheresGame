using UnityEngine;

namespace SpawningSpheresGame.Game.GameRoot.RootManagers
{
    public class WorldContainerComponent : MonoBehaviour
    {
        [SerializeField] private ObjectContainers _worldObjectContainers = new();

        public Transform GetContainerByName(ObjectContainersEnum name)
        {
            var container = _worldObjectContainers.GetContainerByName(name);
            return container;
        }

        public ObjectContainers GetObjectContainers()
        {
            return _worldObjectContainers;
        }
    }
}