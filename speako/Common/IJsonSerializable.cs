public  interface IJsonSerializable<T> where T : class, new()
{
    static T Instance { get; }
}