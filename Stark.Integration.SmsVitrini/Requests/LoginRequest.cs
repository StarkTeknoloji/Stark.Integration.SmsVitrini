using System.Runtime.Serialization;

namespace Stark.Integration.SmsVitrini.Requests
{
    [DataContract]
    public class LoginRequest : BaseRequest
    {
        public LoginRequest(string userName, string password)
            : base(userName, password)
        {
        }
    }
}