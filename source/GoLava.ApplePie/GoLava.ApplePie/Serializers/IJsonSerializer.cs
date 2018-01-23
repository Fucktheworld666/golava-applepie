namespace GoLava.ApplePie.Serializers
{
    public interface IJsonSerializer
    {
        T Deserialize<T>(string json);

        string Serialize(object data);
    }
}