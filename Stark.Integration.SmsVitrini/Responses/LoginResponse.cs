using System.Runtime.Serialization;

namespace Stark.Integration.SmsVitrini.Responses
{
    [DataContract]
    public class LoginResponse : BaseResponse
    {
        [DataMember(Name = "userData")]
        public UserData UserData { get; set; }
    }
}