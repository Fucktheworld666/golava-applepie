using System;
using Newtonsoft.Json;

namespace GoLava.ApplePie.Serializers
{
    public class JsonSerializer
    {
        private readonly JsonSerializerSettings _jsonSerializerSettings;

        public JsonSerializer(JsonSerializerSettings settings)
        {
            _jsonSerializerSettings = settings 
                ?? throw new ArgumentNullException(nameof(settings));
        }

        public T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, _jsonSerializerSettings);
        }

        public string Serialize(object data)
        {
            return JsonConvert.SerializeObject(data, _jsonSerializerSettings);
        }
    }
}
