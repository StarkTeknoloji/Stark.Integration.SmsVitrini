namespace Stark.Integration.SmsVitrini
{
    public class SimpleJsonSerializer : IJsonSerializer
    {
        public string Serialize(object data)
        {
            return SimpleJson.SimpleJson.SerializeObject(data);
        }

        public T Deserialize<T>(string serializedString)
        {
            return SimpleJson.SimpleJson.DeserializeObject<T>(serializedString);
        }
    }
}