using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SpawningSpheresGame.JsonSerializationExample
{
    public class JsonEntityConverterTest : JsonConverter<EntityJsonTest>
    {
        private static readonly JsonSerializer _entityInternalSerializer = new()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Converters = { new Vector3ConverterTest(), new QuaternionConverterTest() }
        };


        public override void WriteJson(JsonWriter writer, EntityJsonTest value, JsonSerializer serializer)
        {
            _entityInternalSerializer.Serialize(writer, value);
        }


        public override EntityJsonTest ReadJson(JsonReader reader, Type objectType, EntityJsonTest existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var jsonObject = JObject.Load(reader);
            var entity = EntityJsonTypeResolverTest.ParseByType(jsonObject, _entityInternalSerializer);

            return entity;
        }
    }
}