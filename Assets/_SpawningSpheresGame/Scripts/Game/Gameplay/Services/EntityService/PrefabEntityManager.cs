using System;
using System.Collections.Generic;
using SpawningSpheresGame.Game.Common.DataTypes;
using SpawningSpheresGame.Game.Common.DataTypes.Enums;
using UnityEngine;

namespace SpawningSpheresGame.Game.Gameplay.Services
{
    public class PrefabEntityManager : MonoBehaviour
    {
        private readonly Dictionary<Type, List<IView>> _bindersByType = new();
        private readonly HashSet<int> _registeredEntityIds = new();

        [SerializeField] private string _prefabId;
        [SerializeField] private int _instanceId;

        [SerializeField] private List<EntityDebugInfo> _activeEntities = new List<EntityDebugInfo>();
        [SerializeField] private List<string> _activeBinderTypes = new List<string>();
        [SerializeField] private int _totalEntityCount = 0;

        [SerializeField] private List<GameplayEntitiesId> _allowedEntityTypes = new List<GameplayEntitiesId>();

        public string PrefabId => _prefabId;
        public int InstanceId => _instanceId;

        private void Awake()
        {
            if (string.IsNullOrEmpty(_prefabId))
            {
                _prefabId = Guid.NewGuid().ToString();
            }
        }

        public bool CanHostEntityType(GameplayEntitiesId entityId)
        {
            if (_allowedEntityTypes == null || _allowedEntityTypes.Count == 0)
            {
                return true;
            }

            return _allowedEntityTypes.Contains(entityId);
        }

        public void SetInstanceId(int id)
        {
            _instanceId = id;
        }

        public void RegisterBinder(IView binder, Type binderType, int entityId)
        {
            if (!_bindersByType.ContainsKey(binderType))
            {
                _bindersByType[binderType] = new List<IView>();
                _activeBinderTypes.Add(binderType.Name);
            }

            _bindersByType[binderType].Add(binder);
            _registeredEntityIds.Add(entityId);

            _activeEntities.Add(new EntityDebugInfo(entityId, binderType.Name));
            _totalEntityCount = _registeredEntityIds.Count;
        }

        public void UnregisterBinder(IView binder, Type binderType, int entityId)
        {
            if (_bindersByType.TryGetValue(binderType, out var binders))
            {
                binders.Remove(binder);
                if (binders.Count == 0)
                {
                    _bindersByType.Remove(binderType);
                    _activeBinderTypes.Remove(binderType.Name);
                }
            }

            _registeredEntityIds.Remove(entityId);

            _activeEntities.RemoveAll(info => info.EntityId == entityId);
            _totalEntityCount = _registeredEntityIds.Count;
        }

        public bool CanBeDestroyed()
        {
            return _registeredEntityIds.Count == 0;
        }

        public bool HasBinderOfType(Type binderType)
        {
            return _bindersByType.ContainsKey(binderType) && _bindersByType[binderType].Count > 0;
        }

        public T GetBinderOfType<T>() where T : class, IView
        {
            var type = typeof(T);
            if (_bindersByType.TryGetValue(type, out var binders) && binders.Count > 0)
            {
                return binders[0] as T;
            }

            return null;
        }

        public IEnumerable<int> GetRegisteredEntityIds()
        {
            return new HashSet<int>(_registeredEntityIds);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (Application.isPlaying)
            {
                _totalEntityCount = _registeredEntityIds.Count;
            }
        }
#endif
    }

    [Serializable]
    public class EntityDebugInfo
    {
        public int EntityId;
        public string BinderType;

        public EntityDebugInfo(int entityId, string binderType)
        {
            EntityId = entityId;
            BinderType = binderType;
        }
    }
}