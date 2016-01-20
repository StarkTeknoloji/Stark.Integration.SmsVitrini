namespace Stark.Integration.SmsVitrini
{
    public interface IJsonSerializer
    {
        string Serialize<T>(T data);

        T Deserialize<T>(string serializedString);
    }
}