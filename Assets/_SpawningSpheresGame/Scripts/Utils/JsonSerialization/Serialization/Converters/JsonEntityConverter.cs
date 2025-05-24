using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SpawningSpheresGame.Game.State.Entities;

namespace SpawningSpheresGame.Utils.JsonSerialization
{
    public class JsonEntityConverter : JsonConverter<EntityData>
    {
        private static readonly JsonSerializer _entityInternalSerializer = new()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Converters = { new Vector3Converter(), new QuaternionConverter() }
        };


        public override void WriteJson(JsonWriter writer, EntityData value, JsonSerializer serializer)
        {
            _entityInternalSerializer.Serialize(writer, value);
        }


        public override EntityData ReadJson(JsonReader reader, Type objectType, EntityData existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var jsonObject = JObject.Load(reader);
            var entity = JsonEntityTypeResolver.ParseByType(jsonObject, _entityInternalSerializer);

            return entity;
        }
    }
}