using System;
using UnityEngine;
using Zenject;

namespace SpawningSpheresGame.Game.GameRoot.RootManagers
{
    public class RootSceneContainersManager : IDisposable
    {
        public ObjectContainers WorldObjectContainers { get; private set; }
        public ObjectContainers UIObjectContainers { get; private set; }

        private bool _worldInitialFlag = false;
        private bool _uiInitialFlag = false;

        private DiContainer _container;

        public RootSceneContainersManager(DiContainer container)
        {
            _container = container;
        }

        public void InitWorldContainers(ObjectContainers worldObjectContainers)
        {
            WorldObjectContainers = null;
            _worldInitialFlag = false;

            WorldObjectContainers = worldObjectContainers;

            if (WorldObjectContainers != null)
                _worldInitialFlag = true;
        }

        public void InitUIContainers(ObjectContainers uiObjectContainers)
        {
            UIObjectContainers = null;
            _uiInitialFlag = false;

            UIObjectContainers = uiObjectContainers;

            if (UIObjectContainers != null)
                _uiInitialFlag = true;
        }

        public Transform GetWorldContainerByName(ObjectContainersEnum name)
        {
            if (!_worldInitialFlag)
            {
                throw new Exception("WorldContainer wasn't inined!");
            }

            return WorldObjectContainers.GetContainerByName(name);
        }

        public Transform GetUIContainerByName(ObjectContainersEnum name)
        {
            if (!_uiInitialFlag)
            {
                throw new Exception("WorldContainer wasn't inined!");
            }

            return UIObjectContainers.GetContainerByName(name);
        }

        public void Dispose()
        {
            WorldObjectContainers = null;
            UIObjectContainers = null;
        }
    }
}