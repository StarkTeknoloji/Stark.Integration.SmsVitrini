using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Stark.Integration.SmsVitrini.Requests
{
    [DataContract]
    public class MessageData
    {
        [DataMember(Name = "tel")]
        public List<string> Numbers { get; set; }

        [DataMember(Name = "msg")]
        public string Text { get; set; }
    }
}