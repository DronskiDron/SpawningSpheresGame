
using Newtonsoft.Json;

namespace SpawningSpheresGame.Utils.JsonSerialization
{
    public class JsonProjectSettings
    {
        public static void ApplyProjectSerializationSettings()
        {
            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new JsonEntityConverter());
            settings.Converters.Add(new Vector3Converter());
            settings.Converters.Add(new QuaternionConverter());


            JsonConvert.DefaultSettings = () => settings;
        }
    }
}
