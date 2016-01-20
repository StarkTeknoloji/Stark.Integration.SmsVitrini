namespace Stark.Integration.SmsVitrini.Requests
{
    public class BaseRequest
    {
        public BaseRequest(string userName, string password)
        {
            user = new AuthenticationData(userName, password);
        }

        public AuthenticationData user { get; set; }
    }
}