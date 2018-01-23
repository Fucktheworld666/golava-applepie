using System;
using System.Security;
using GoLava.ApplePie.Security;
using Newtonsoft.Json;

namespace GoLava.ApplePie.Contracts.Converters
{
    /// <summary>
    /// Deserializes a <see cref="T:SecureString"/> to JSON.
    /// </summary>
    public class SecureStringJsonConverter : JsonConverter
    {
        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>
        /// <c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.
        /// </returns>
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(SecureString);
        }

        /// <summary>
        /// Reads the JSON representation of the object.
        /// </summary>
        /// <param name="reader">The <see cref="T:Newtonsoft.Json.JsonReader" /> to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value of object being read.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>
        /// The object value.
        /// </returns>
        /// <remarks>Reading json to secure string is not supported. Calling this function will throw an exception.</remarks>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter" /> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if(writer == null)
                throw new ArgumentNullException(nameof(writer));
            
            var secureString = value as SecureString;
            if (secureString == null)
            {
                writer.WriteNull();
            }
            else
            {
                var secureStringConverter = new SecureStringConverter();
                writer.WriteValue(secureStringConverter.ConvertSecureStringToPlainString(secureString));
            }
        }
    }
}