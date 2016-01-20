namespace Stark.Integration.SmsVitrini
{
    public class SimpleJsonSerializer : IJsonSerializer
    {
        public string Serialize<T>(T data)
        {
            throw new System.NotImplementedException();
        }

        public T Deserialize<T>(string serializedString)
        {
            throw new System.NotImplementedException();
        }
    }
}