using System;
using System.Collections.Generic;
using System.Text;
using Stark.Integration.SmsVitrini.Models;
using Stark.Integration.SmsVitrini.Requests;

namespace Stark.Integration.SmsVitrini
{
    public class SmsClient
    {
        private readonly string _userName;
        private readonly string _password;
        private readonly TimeSpan _timeOut;
        private readonly IJsonSerializer _serializer;
        private readonly IPhoneNumberValidator _phoneNumberValidator;

        public SmsClient(string userName, string password)
            : this(userName, password, TimeSpan.FromSeconds(30))
        {
        }

        public SmsClient(string userName, string password, TimeSpan timeOut)
            : this(userName, password, timeOut, new TurkeyPhoneNumberValidator())
        {
        }

        public SmsClient(string userName, string password, TimeSpan timeOut, IPhoneNumberValidator phoneNumberValidator)
            : this(userName, password, timeOut, phoneNumberValidator, new SimpleJsonSerializer())
        {
        }

        public SmsClient(string userName, string password, TimeSpan timeOut, IPhoneNumberValidator phoneNumberValidator, IJsonSerializer serializer)
        {
            if (String.IsNullOrEmpty(userName))
            {
                throw new ArgumentNullException("userName");
            }

            if (String.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException("password");
            }

            if (phoneNumberValidator == null)
            {
                throw new ArgumentNullException("phoneNumberValidator");
            }

            if (serializer == null)
            {
                throw new ArgumentNullException("serializer");
            }

            _userName = userName;
            _password = password;
            _timeOut = timeOut;
            _serializer = serializer;
            _phoneNumberValidator = phoneNumberValidator;
        }

        public ServiceResult<MessageResponse> Send(string originator, Message message, bool includeSpecialTurkishCharacters = false)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            List<Message> messages = new List<Message>();
            messages.Add(message);
            return Send(originator, messages, includeSpecialTurkishCharacters);
        }

        public ServiceResult<MessageResponse> Send(string originator, List<Message> messages, bool includeSpecialTurkishCharacters = false)
        {
            if (String.IsNullOrEmpty(originator))
            {
                throw new ArgumentNullException("originator");
            }

            SmsRequest smsRequest = new SmsRequest(_userName, _password, originator, messages, includeSpecialTurkishCharacters);
            string jsonString = _serializer.Serialize(smsRequest);
            byte[] byteArray = Encoding.UTF8.GetBytes(jsonString);
            string base64String = Convert.ToBase64String(byteArray);
            string requestString = String.Concat("data=", base64String);

            string response = HttpPost("http://api.mesajpaneli.com/json_api/", requestString);
            byteArray = Convert.FromBase64String(response);
            response = Encoding.UTF8.GetString(byteArray);
            //WebRequest request = WebRequest.Create();
            //request.Method = "POST";
            //request.ContentType = "application/x-www-form-urlencoded";
            //WebResponse response = request.
            return null;
        }

        public static string HttpPost(string URI, string Parameters)
        {
            System.Net.WebRequest req = System.Net.WebRequest.Create(URI);
            //Add these, as we're doing a POST
            req.ContentType = "application/x-www-form-urlencoded";
            req.Method = "POST";
            //We need to count how many bytes we're sending. Post'ed Faked Forms should be name=value&
            byte[] bytes = System.Text.Encoding.ASCII.GetBytes(Parameters);
            req.ContentLength = bytes.Length;
            System.IO.Stream os = req.GetRequestStream();
            os.Write(bytes, 0, bytes.Length); //Push it out there
            os.Close();
            System.Net.WebResponse resp = req.GetResponse();
            if (resp == null) return null;
            System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
            return sr.ReadToEnd().Trim();
        }

        public ServiceResult<CustomerDetail> GetCustomerDetails()
        {
            throw new NotImplementedException();
        }

        public ServiceResult<List<ReportItem>> GetReports(string smsReferenceNo)
        {
            throw new NotImplementedException();
        }
    }
}