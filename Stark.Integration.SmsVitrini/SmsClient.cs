using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Stark.Integration.SmsVitrini.Models;
using Stark.Integration.SmsVitrini.Requests;
using Stark.Integration.SmsVitrini.Responses;

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
            SmsResponse smsResponse = Post<SmsResponse>("http://api.mesajpaneli.com/json_api/", smsRequest);
            return null;
        }

        public ServiceResult<CustomerDetail> GetCustomerDetails()
        {
            ServiceResult<CustomerDetail> result = new ServiceResult<CustomerDetail>();

            LoginRequest loginRequest = new LoginRequest(_userName, _password);
            LoginResponse loginResponse = Post<LoginResponse>("http://api.mesajpaneli.com/json_api/login", loginRequest);

            if (!loginResponse.status)
            {
                result.Success = false;
                result.Message = loginResponse.error;
            }
            else
            {
                result.Success = true;
                result.Data = new CustomerDetail()
                {
                    CustomerId = loginResponse.userData.musteriid,
                    Credits = Convert.ToInt32(loginResponse.userData.orjinli)
                };
            }

            return result;
        }

        public ServiceResult<List<ReportItem>> GetReports(string smsReferenceNo)
        {
            throw new NotImplementedException();
        }

        private T Post<T>(string url, object request) where T : BaseResponse, new()
        {
            string jsonString = _serializer.Serialize(request);
            byte[] byteArray = Encoding.UTF8.GetBytes(jsonString);
            string base64String = Convert.ToBase64String(byteArray);
            string requestString = String.Concat("data=", base64String);
            WebRequest req = WebRequest.Create(url);
            req.Timeout = (int)_timeOut.TotalMilliseconds;
            req.ContentType = "application/x-www-form-urlencoded";
            req.Method = "POST";
            byte[] bytes = Encoding.UTF8.GetBytes(requestString);
            req.ContentLength = bytes.Length;
            Stream os = req.GetRequestStream();
            os.Write(bytes, 0, bytes.Length);
            os.Close();
            WebResponse resp = req.GetResponse();
            Stream responseStream = resp.GetResponseStream();

            if (responseStream != null)
            {
                StreamReader sr = new StreamReader(responseStream);
                string response = sr.ReadToEnd().Trim();
                byteArray = Convert.FromBase64String(response);
                response = Encoding.UTF8.GetString(byteArray);
                return _serializer.Deserialize<T>(response);
            }

            T defaultResponse = new T
            {
                status = false,
                error = "Cannot get any response from service."
            };

            return defaultResponse;
        }
    }
}