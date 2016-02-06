using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Stark.Integration.SmsVitrini.Responses
{
    [DataContract]
    public class ReportDetailItemContainer
    {
        [DataMember(Name = "bekleyen")]
        public List<string> WaitingNumberReportDetailItems { get; set; }

        [DataMember(Name = "gonderilmis")]
        public List<ReportDetailItem> SentNumberReportDetailItems { get; set; }

        [DataMember(Name = "hata")]
        public List<ReportDetailItem> FailedNumberReportDetailItems { get; set; }
    }
}