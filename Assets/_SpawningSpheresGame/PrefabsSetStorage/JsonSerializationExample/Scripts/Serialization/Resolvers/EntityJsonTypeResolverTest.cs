using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SpawningSpheresGame.JsonSerializationExample
{
    public static class EntityJsonTypeResolverTest
    {
        public static EntityJsonTest ParseByType(JObject jsonObject, JsonSerializer jsonSerializer)
        {
            var type = jsonObject["Type"].ToObject<EntityTypeJsonTest>();

            return type switch
            {
                EntityTypeJsonTest.Character => jsonObject.ToObject<CharacterEntityJsonTest>(jsonSerializer),
                EntityTypeJsonTest.Door => jsonObject.ToObject<DoorEntityJsonTest>(jsonSerializer),
                EntityTypeJsonTest.PickubleItem => jsonObject.ToObject<PickubleItemEntityJsonTest>(jsonSerializer),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}