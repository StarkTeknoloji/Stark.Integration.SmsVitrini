using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Stark.Integration.SmsVitrini.Models;

namespace Stark.Integration.SmsVitrini.Requests
{
    [DataContract]
    public class SmsRequest : BaseRequest
    {
        public SmsRequest(string userName, string password, string originator, List<Message> messages, bool includeTurkishCharacters)
            : base(userName, password)
        {
            Originator = originator;
            IncludeSpecialTurkishCharacters = includeTurkishCharacters;
            Messages = messages.Select(m => new MessageData()
            {
                Text = m.Text,
                Numbers = m.Numbers
            }).ToList();
        }

        [DataMember(Name = "msgBaslik")]
        public string Originator { get; set; }

        [DataMember(Name = "msgData")]
        public List<MessageData> Messages { get; set; }

        [DataMember(Name = "tr")]
        public bool IncludeSpecialTurkishCharacters { get; set; }
    }
}