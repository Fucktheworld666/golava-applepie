using System;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GoLava.ApplePie.Contracts.Converters
{
    public class MillisecondsEpochConverter : DateTimeConverterBase
    {
        private static readonly DateTime _epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var milliseconds = ((DateTime)value - _epoch).TotalMilliseconds;
            writer.WriteRawValue(milliseconds.ToString(CultureInfo.InvariantCulture));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null) 
                return null;
            var milliseconds = (long)reader.Value;
            var dt = _epoch.AddMilliseconds(milliseconds);
            return dt;
        }
    }
}