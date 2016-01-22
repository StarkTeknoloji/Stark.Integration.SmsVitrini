using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Stark.Integration.SmsVitrini.Responses
{
    [DataContract]
    public class UserData
    {
        [DataMember(Name = "musteriid")]
        public string CustomerId { get; set; }

        [DataMember(Name = "bayiid")]
        public string ResellerId { get; set; }

        [DataMember(Name = "musterikodu")]
        public string CustomerCode { get; set; }

        [DataMember(Name = "yetkiliadsoyad")]
        public string AuthorizedPerson { get; set; }

        [DataMember(Name = "firma")]
        public string CompanyName { get; set; }

        [DataMember(Name = "orjinli")]
        public string Credits { get; set; }

        [DataMember(Name = "sistem_kredi")]
        public string StandartCredits { get; set; }

        [DataMember(Name = "basliklar")]
        public List<string> Originators { get; set; }
    }
}