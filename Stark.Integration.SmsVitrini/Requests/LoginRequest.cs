namespace Stark.Integration.SmsVitrini.Requests
{
    public class LoginRequest : BaseRequest
    {
        public LoginRequest(string userName, string password)
            : base(userName, password)
        {
        }
    }
}