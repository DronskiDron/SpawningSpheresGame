using System;
using System.Collections.Generic;
using PrefabsSetStorage.ECS.Components;

namespace PrefabsSetStorage.ECS.ClassicEntity
{
    public class Entity
    {
        public int Id { get; private set; }
        private Dictionary<Type, IComponent> _components = new();

        public Entity(int id) => Id = id;

        public void AddComponent<T>(T component) where T : IComponent
        {
            _components[typeof(T)] = component;
        }

        public T GetComponent<T>() where T : IComponent
        {
            return (T)_components[typeof(T)];
        }

        public bool HasComponent<T>() where T : IComponent
        {
            return _components.ContainsKey(typeof(T));
        }

        public void UpdateComponent<T>(T component) where T : IComponent
        {
            _components[typeof(T)] = component;
        }
    }
}