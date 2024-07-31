using System;
using System.IO;
using Newtonsoft.Json;

public abstract class JsonSerializable<T> : IJsonSerializable<T> where T : JsonSerializable<T>, new()
{
    private static readonly Lazy<T> _instance = new Lazy<T>(() => Load());

    public static T Instance => _instance.Value;

    public void Save()
    {
        var json = JsonConvert.SerializeObject(this, Formatting.Indented);
        File.WriteAllText(GetFilePath(), json);
    }

    protected static string GetFilePath()
    {
        var tempInstance = new T();
        return tempInstance.FilePath;
    }

    protected abstract string FilePath { get; }

    private static T Load()
    {
        var filePath = GetFilePath();
        if (File.Exists(filePath))
        {
            var json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<T>(json) ?? new T();
        }
        return new T();
    }
}
