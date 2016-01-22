using System;
using Stark.Integration.SmsVitrini.Models.Enums;

namespace Stark.Integration.SmsVitrini.Models
{
    public class ReportItem
    {
        public DateTime? DeliveredOn { get; set; }

        public string PhoneNumber { get; set; }

        public OperatorEnum Operator { get; set; }

        public MessageStatusEnum Status { get; set; }

        public FailReasonEnum? FailReason { get; set; }
    }
}