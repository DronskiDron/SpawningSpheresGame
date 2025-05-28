using System.Collections.Generic;
using PrefabsSetStorage.ECS.ClassicEntity;
using PrefabsSetStorage.ECS.Components;

namespace PrefabsSetStorage.ECS.Systems
{
    public class PositionSystem
    {
        private List<Entity> _entities;

        public PositionSystem(List<Entity> entities)
        {
            _entities = entities;
        }

        public void Update()
        {
            foreach (var entity in _entities)
            {
                if (entity.HasComponent<PositionComponent>() &&
                    entity.HasComponent<RenderComponent>())
                {
                    var position = entity.GetComponent<PositionComponent>();
                    var render = entity.GetComponent<RenderComponent>();

                    render.GameObject.transform.position = position.Position;
                }
            }
        }
    }
}