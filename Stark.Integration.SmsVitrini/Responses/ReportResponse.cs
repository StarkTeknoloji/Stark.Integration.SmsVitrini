using System.Runtime.Serialization;

namespace Stark.Integration.SmsVitrini.Responses
{
    [DataContract]
    public class ReportResponse : BaseResponse
    {
        [DataMember(Name = "refData")]
        public ReportData Data { get; set; }
    }
}