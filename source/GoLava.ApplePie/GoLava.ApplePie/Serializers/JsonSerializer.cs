using System;
using Newtonsoft.Json;

namespace GoLava.ApplePie.Serializers
{
    /// <summary>
    /// Serialize and deserialize JSON encoded strings.
    /// </summary>
    public class JsonSerializer
    {
        private readonly JsonSerializerSettings _jsonSerializerSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:JsonSerializer"/> class.
        /// </summary>
        public JsonSerializer()
            : this(new JsonSerializerSettings()) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:JsonSerializer"/> class.
        /// </summary>
        /// <param name="settings">An instance of <see cref="T:JsonSerializerSettings"/> to 
        /// be used when serializing/deserializing.</param>
        public JsonSerializer(JsonSerializerSettings settings)
        {
            _jsonSerializerSettings = settings 
                ?? throw new ArgumentNullException(nameof(settings));
        }

        /// <summary>
        /// Deserialize the specified json string to an object of the given generic type.
        /// </summary>
        /// <returns>The deserialize.</returns>
        /// <param name="json">The json string to deserialize..</param>
        public T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, _jsonSerializerSettings);
        }

        /// <summary>
        /// Serialize the specified object to a json string.
        /// </summary>
        /// <param name="data">The object to be serialized.</param>
        public string Serialize(object data)
        {
            return JsonConvert.SerializeObject(data, _jsonSerializerSettings);
        }
    }
}