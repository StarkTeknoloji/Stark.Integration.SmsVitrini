using System.Collections.Generic;
using System.Linq;
using Stark.Integration.SmsVitrini.Models;

namespace Stark.Integration.SmsVitrini.Requests
{
    public class SmsRequest : BaseRequest
    {
        public SmsRequest(string userName, string password, string originator, List<Message> messages, bool includeTurkishCharacters)
            : base(userName, password)
        {
            msgBaslik = originator;
            tr = includeTurkishCharacters;
            msgData = messages.Select(m => new MessageData()
            {
                msg = m.Text,
                tel = m.Numbers
            }).ToList();
        }

        public string msgBaslik { get; set; }

        public List<MessageData> msgData { get; set; }

        public bool tr { get; set; }
    }
}