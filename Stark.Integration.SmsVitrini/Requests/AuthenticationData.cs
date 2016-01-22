using System.Runtime.Serialization;

namespace Stark.Integration.SmsVitrini.Requests
{
    [DataContract]
    public class AuthenticationData
    {
        public AuthenticationData(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }

        [DataMember(Name = "name")]
        public string UserName { get; set; }

        [DataMember(Name = "pass")]
        public string Password { get; set; }
    }
}