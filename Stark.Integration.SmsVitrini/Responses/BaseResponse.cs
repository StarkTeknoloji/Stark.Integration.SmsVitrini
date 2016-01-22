using System.Runtime.Serialization;

namespace Stark.Integration.SmsVitrini.Responses
{
    [DataContract]
    public class BaseResponse
    {
        [DataMember(Name = "status")]
        public bool Success { get; set; }

        [DataMember(Name = "error")]
        public string ErrorMessage { get; set; }
    }
}