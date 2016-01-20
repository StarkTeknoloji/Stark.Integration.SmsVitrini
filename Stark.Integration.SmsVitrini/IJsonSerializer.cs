﻿namespace Stark.Integration.SmsVitrini
{
    public interface IJsonSerializer
    {
        string Serialize(object data);

        T Deserialize<T>(string serializedString);
    }
}