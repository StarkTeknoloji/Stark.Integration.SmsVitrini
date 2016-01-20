namespace Stark.Integration.SmsVitrini.Requests
{
    public class AuthenticationData
    {
        public AuthenticationData(string userName, string password)
        {
            name = userName;
            pass = password;
        }

        public string name { get; set; }

        public string pass { get; set; }
    }
}