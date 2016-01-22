using System.Runtime.Serialization;

namespace Stark.Integration.SmsVitrini.Requests
{
    [DataContract]
    public class BaseRequest
    {
        public BaseRequest(string userName, string password)
        {
            User = new AuthenticationData(userName, password);
        }

        [DataMember(Name = "user")]
        public AuthenticationData User { get; set; }
    }
}