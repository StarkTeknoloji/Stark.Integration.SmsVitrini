using System.Runtime.Serialization;

namespace Stark.Integration.SmsVitrini.Requests
{
    [DataContract]
    public class ReportRequest : BaseRequest
    {
        public ReportRequest(string userName, string password, string smsReferenceNo, bool excludeOperators = false, bool excludeDates = false) 
            : base(userName, password)
        {
            SmsReferenceNo = smsReferenceNo;
            IncludeOperators = !excludeOperators;
            IncludeDates = !excludeDates;
        }

        [DataMember(Name = "refno")]
        public string SmsReferenceNo { get; set; }

        [DataMember(Name = "operators")]
        public bool IncludeOperators { get; set; }

        [DataMember(Name = "dates")]
        public bool IncludeDates { get; set; }
    }
}