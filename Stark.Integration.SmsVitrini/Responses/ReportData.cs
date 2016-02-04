using System.Runtime.Serialization;

namespace Stark.Integration.SmsVitrini.Responses
{
    [DataContract]
    public class ReportData
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "gateway_cevap")]
        public string GatewayResponse { get; set; }

        [DataMember(Name = "baslik")]
        public string Originator { get; set; }

        [DataMember(Name = "mesaj")]
        public string Text { get; set; }

        [DataMember(Name = "numaralar")]
        public ReportDetailItemContainer ReportDetailItemContainer { get; set; }

        [DataMember(Name = "hata")]
        public string ErrorMessage { get; set; }
    }
}