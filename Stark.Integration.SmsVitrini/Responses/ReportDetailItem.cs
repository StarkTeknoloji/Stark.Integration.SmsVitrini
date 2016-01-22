using System;
using System.Runtime.Serialization;

namespace Stark.Integration.SmsVitrini.Responses
{
    [DataContract]
    public class ReportDetailItem
    {
        [DataMember(Name = "tel")]
        public string Number { get; set; }

        [DataMember(Name = "status")]
        public string StatusText { get; set; }

        [DataMember(Name = "donedate")]
        public string CompletedOn { get; set; }

        [DataMember(Name = "operator")]
        public string Operator { get; set; }

        [DataMember(Name = "gsmerror")]
        public string FailReason { get; set; }
    }
}