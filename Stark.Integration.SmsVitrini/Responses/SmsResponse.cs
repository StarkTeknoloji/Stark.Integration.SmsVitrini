using System.Runtime.Serialization;

namespace Stark.Integration.SmsVitrini.Responses
{
    [DataContract]
    public class SmsResponse : BaseResponse
    {
        [DataMember(Name = "ref")]
        public long SmsReferenceNo { get; set; }
    }
}