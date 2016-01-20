using System.Collections.Generic;

namespace Stark.Integration.SmsVitrini.Models
{
    public class Message
    {
        public List<long> Numbers { get; set; }
        
        public string Text { get; set; }
    }
}
