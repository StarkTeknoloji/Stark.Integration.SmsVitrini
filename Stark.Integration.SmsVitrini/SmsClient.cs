using System;
using System.Collections.Generic;
using Stark.Integration.SmsVitrini.Models;

namespace Stark.Integration.SmsVitrini
{
    public class SmsClient
    {
        private readonly string _userName;
        private readonly string _password;
        private readonly TimeSpan _timeOut;
        private readonly IJsonSerializer _serializer;

        public SmsClient(string userName, string password)
            : this(userName, password, TimeSpan.FromSeconds(30))
        {
        }

        public SmsClient(string userName, string password, TimeSpan timeOut)
            : this(userName, password, timeOut, new SimpleJsonSerializer())
        {
        }

        public SmsClient(string userName, string password, TimeSpan timeOut, IJsonSerializer serializer)
        {
            _userName = userName;
            _password = password;
            _timeOut = timeOut;
            _serializer = serializer;
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
            throw new NotImplementedException();
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