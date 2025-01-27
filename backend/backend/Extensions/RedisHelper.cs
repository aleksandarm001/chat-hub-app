using Newtonsoft.Json;
using StackExchange.Redis;

namespace backend.Extensions;

public class RedisHelper
{
    public static HashEntry[] ToHashEntries<T>(T obj)
    {
        var properties = obj.GetType().GetProperties();
        var hashEntries = new List<HashEntry>();

        foreach (var property in properties)
        {
            var value = property.GetValue(obj);

            if (value != null)
            {
                var serializedValue = value is string || value.GetType().IsValueType
                    ? value.ToString()
                    : JsonConvert.SerializeObject(value);

                hashEntries.Add(new HashEntry(property.Name, serializedValue));
            }
        }

        return hashEntries.ToArray();
    }

    public static T FromHashEntries<T>(HashEntry[] hashEntries) where T : new()
    {
        var obj = new T();
        var properties = typeof(T).GetProperties();

        foreach (var entry in hashEntries)
        {
            var property = Array.Find(properties, p => p.Name == entry.Name);
            if (property != null)
            {
                var value = entry.Value.ToString();

                if (property.PropertyType == typeof(string) || property.PropertyType.IsValueType)
                {
                    property.SetValue(obj, Convert.ChangeType(value, property.PropertyType));
                }
                else
                {
                    var deserializedValue = JsonConvert.DeserializeObject(value, property.PropertyType);
                    property.SetValue(obj, deserializedValue);
                }
            }
        }

        return obj;
    }
}