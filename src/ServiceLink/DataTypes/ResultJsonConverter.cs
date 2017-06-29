using System;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ServiceLink.Exceptions;

namespace ServiceLink
{
    public class ResultJsonConverter : JsonConverter
    {
        private static readonly MethodInfo WriteMethod = typeof(ResultJsonConverter).GetRuntimeMethods()
            .First(p => p.Name == nameof(WriteResult));

        private static readonly MethodInfo ReadMethod = typeof(ResultJsonConverter).GetRuntimeMethods()
            .First(p => p.Name == nameof(ReadResult));

        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            var typeInfo = objectType.GetTypeInfo();
            var method = ReadMethod.MakeGenericMethod(typeInfo.GenericTypeArguments);
            return method.Invoke(null, new object[] {reader, serializer});
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var typeInfo = value.GetType().GetTypeInfo();
            var method = WriteMethod.MakeGenericMethod(typeInfo.GenericTypeArguments);
            method.Invoke(null, new[] {writer, value, serializer});
        }

        public static void WriteResult<T>(JsonWriter writer, Result<T> result, JsonSerializer serializer)
        {
            var (successName, errorName) = PropertyNames<T>(serializer);
            if (result is Result<T>.Success ok)
            {
                writer.WriteStartObject();
                writer.WritePropertyName(successName);
                serializer.Serialize(writer, ok.Value);
                writer.WriteEndObject();
            }
            else if (result is Result<T>.Error er)
            {
                writer.WriteStartObject();
                writer.WritePropertyName(errorName);
                if (er.Value is SerializedException se)
                    serializer.Serialize(writer, se);
                else
                    serializer.Serialize(writer, new JsonSerializedException(er.Value));
                writer.WriteEndObject();
            }
        }


        public static Result<T> ReadResult<T>(JsonReader reader, JsonSerializer serializer)
        {
            (string, string) ToLower((string, string) tuple)
            {
                return (tuple.Item1.ToLower(), tuple.Item2.ToLower());
            }

            var (successName, errorName) = ToLower(PropertyNames<T>(serializer));
            reader.Read();

            var propName = reader.Value.ToString().ToLower();
            Result<T> result;
            reader.Read();
            if (propName == successName)
            {
                var val = serializer.Deserialize<T>(reader);
                result = val.ToSuccess();
            }
            else if (propName == errorName)
            {
                var err = serializer.Deserialize<SerializedException>(reader);
                result = err.ToError<T>();
            }
            else
            {
                throw new JsonSerializationException("Invalid result format");
            }
            reader.Read();
            return result;
        }

        private static (string successName, string errorName) PropertyNames<T>(JsonSerializer serializer)
        {
            var namingStrategy = (serializer.ContractResolver as DefaultContractResolver)?.NamingStrategy;
            const string successName = nameof(Result<T>.Success);
            const string errorName = nameof(Result<T>.Error);
            return namingStrategy == null
                ? (successName, errorName)
                : (namingStrategy.GetPropertyName(successName, false), namingStrategy.GetPropertyName(errorName,
                    false));
        }
    }
}