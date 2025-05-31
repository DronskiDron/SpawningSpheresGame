using System.Collections.Generic;
using PrefabsSetStorage.ECS.ClassicEntity;
using PrefabsSetStorage.ECS.Components;
using PrefabsSetStorage.ECS.Systems;
using UnityEngine;

namespace PrefabsSetStorage.ECS
{
    public class ECSGameManager : MonoBehaviour
    {
        private List<Entity> _entities = new();
        private RotationSystem _rotationSystem;
        private PositionSystem _positionSystem;

        void Start()
        {
            CreateRotatingCube();

            _rotationSystem = new RotationSystem(_entities);
            _positionSystem = new PositionSystem(_entities);
        }

        void CreateRotatingCube()
        {
            GameObject cubeObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cubeObject.name = "Rotating Cube";

            var entity = new Entity(1);

            entity.AddComponent(new PositionComponent
            {
                Position = Vector3.zero
            });

            entity.AddComponent(new RotationComponent
            {
                Rotation = Vector3.zero
            });

            entity.AddComponent(new RotationSpeedComponent
            {
                RotationSpeed = new Vector3(0, 90f, 45f)
            });

            entity.AddComponent(new RenderComponent
            {
                GameObject = cubeObject
            });

            _entities.Add(entity);
        }

        void Update()
        {
            _rotationSystem.Update();
            _positionSystem.Update();
        }

        void OnDestroy()
        {
            foreach (var entity in _entities)
            {
                if (entity.HasComponent<RenderComponent>())
                {
                    var render = entity.GetComponent<RenderComponent>();
                    if (render.GameObject != null)
                        Destroy(render.GameObject);
                }
            }
            _entities.Clear();
        }
    }
}