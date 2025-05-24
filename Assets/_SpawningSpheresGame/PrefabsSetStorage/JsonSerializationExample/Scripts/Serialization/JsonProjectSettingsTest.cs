using Newtonsoft.Json;

namespace SpawningSpheresGame.JsonSerializationExample
{
    public static class JsonProjectSettingsTest
    {
        public static void ApplyProjectSerializationSettings()
        {
            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new JsonEntityConverterTest());
            settings.Converters.Add(new Vector3ConverterTest());
            settings.Converters.Add(new QuaternionConverterTest());


            JsonConvert.DefaultSettings = () => settings;
        }
    }
}