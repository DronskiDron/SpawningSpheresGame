using System.Collections.Generic;
using PrefabsSetStorage.ECS.ClassicEntity;
using PrefabsSetStorage.ECS.Components;
using UnityEngine;

namespace PrefabsSetStorage.ECS.Systems
{
    public class RotationSystem
    {
        private List<Entity> _entities;

        public RotationSystem(List<Entity> entities)
        {
            _entities = entities;
        }

        public void Update()
        {
            foreach (var entity in _entities)
            {
                if (entity.HasComponent<RotationComponent>() &&
                    entity.HasComponent<RotationSpeedComponent>())
                {
                    var rotation = entity.GetComponent<RotationComponent>();
                    var rotationSpeed = entity.GetComponent<RotationSpeedComponent>();

                    rotation.Rotation += rotationSpeed.RotationSpeed * Time.deltaTime;

                    rotation.Rotation = new Vector3(
                        rotation.Rotation.x % 360f,
                        rotation.Rotation.y % 360f,
                        rotation.Rotation.z % 360f
                    );

                    entity.UpdateComponent(rotation);

                    if (entity.HasComponent<RenderComponent>())
                    {
                        var render = entity.GetComponent<RenderComponent>();
                        render.GameObject.transform.rotation = Quaternion.Euler(rotation.Rotation);
                    }
                }
            }
        }
    }
}