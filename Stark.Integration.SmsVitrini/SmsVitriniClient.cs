using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Stark.Integration.SmsVitrini.Models;
using Stark.Integration.SmsVitrini.Models.Enums;
using Stark.Integration.SmsVitrini.Requests;
using Stark.Integration.SmsVitrini.Responses;

namespace Stark.Integration.SmsVitrini
{
    public class SmsVitriniClient
    {
        private readonly string _userName;
        private readonly string _password;
        private readonly TimeSpan _timeOut;
        private readonly IJsonSerializer _serializer;
        private readonly IPhoneNumberValidator _phoneNumberValidator;

        public SmsVitriniClient(string userName, string password)
            : this(userName, password, TimeSpan.FromSeconds(30))
        {
        }

        public SmsVitriniClient(string userName, string password, TimeSpan timeOut)
            : this(userName, password, timeOut, null)
        {
        }

        public SmsVitriniClient(string userName, string password, TimeSpan timeOut, IPhoneNumberValidator phoneNumberValidator)
            : this(userName, password, timeOut, phoneNumberValidator, new SimpleJsonSerializer())
        {
        }

        public SmsVitriniClient(string userName, string password, TimeSpan timeOut, IPhoneNumberValidator phoneNumberValidator, IJsonSerializer serializer)
        {
            if (String.IsNullOrEmpty(userName))
            {
                throw new ArgumentNullException("userName");
            }

            if (String.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException("password");
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
                return new ServiceResult<MessageResponse>()
                {
                    Success = false,
                    Message = "You need to provide at least 1 Message to send."
                };
            }

            List<Message> messages = new List<Message> { message };
            return Send(originator, messages, includeSpecialTurkishCharacters);
        }

        public ServiceResult<MessageResponse> Send(string originator, List<Message> messages, bool includeSpecialTurkishCharacters = false)
        {
            ServiceResult<MessageResponse> result = new ServiceResult<MessageResponse>();

            if (String.IsNullOrEmpty(originator))
            {
                result = new ServiceResult<MessageResponse>()
                {
                    Success = false,
                    Message = "You need to provider an originator."
                };
            }

            if (messages == null || !messages.Any())
            {
                return new ServiceResult<MessageResponse>()
                {
                    Success = false,
                    Message = "You need to provide at least 1 Message to send."
                };
            }

            if (_phoneNumberValidator != null)
            {
                messages = GetValidMessages(messages);
            }

            if (messages == null || !messages.Any())
            {
                return new ServiceResult<MessageResponse>()
                {
                    Success = false,
                    Message = "You need to provide at least 1 valid Message to send. Check your phone numbers and message texts."
                };
            }

            SmsRequest smsRequest = new SmsRequest(_userName, _password, originator, messages, includeSpecialTurkishCharacters);
            SmsResponse smsResponse = Post<SmsResponse>("http://api.mesajpaneli.com/json_api/", smsRequest);

            if (smsResponse.Success)
            {
                result.Success = true;
                result.Data = new MessageResponse()
                {
                    SmsReferenceNo = smsResponse.SmsReferenceNo.ToString()
                };
            }
            else
            {
                result.Success = false;
                result.Message = smsResponse.ErrorMessage;
                result.ErrorCode = GetErrorCode(smsResponse);
            }

            return result;
        }

        public ServiceResult<CustomerDetail> GetCustomerDetails()
        {
            ServiceResult<CustomerDetail> result = new ServiceResult<CustomerDetail>();

            LoginRequest loginRequest = new LoginRequest(_userName, _password);
            LoginResponse loginResponse = Post<LoginResponse>("http://api.mesajpaneli.com/json_api/login", loginRequest);

            if (!loginResponse.Success)
            {
                result.Success = false;
                result.Message = loginResponse.ErrorMessage;
            }
            else
            {
                result.Success = true;
                result.Data = new CustomerDetail()
                {
                    CustomerId = loginResponse.UserData.CustomerId,
                    Credits = Convert.ToInt32(loginResponse.UserData.Credits)
                };
            }

            return result;
        }

        public ServiceResult<List<ReportItem>> GetReports(string smsReferenceNo)
        {
            ServiceResult<List<ReportItem>> result = new ServiceResult<List<ReportItem>>();

            ReportRequest reportRequest = new ReportRequest(_userName, _password, smsReferenceNo);
            ReportResponse reportResponse = Post<ReportResponse>("http://api.mesajpaneli.com/json_api/report", reportRequest);

            if (reportResponse == null)
            {
                result.Success = false;
                result.Message = "Cannot get any response from service.";
            }
            else if (!reportResponse.Success || reportResponse.Data == null || reportResponse.Data.ReportDetailItemContainer == null)
            {
                result.Success = false;
                result.Message = String.Format("There is a problem with the service response. Check the response string for more information: {0}", reportResponse.ResponseString);
            }
            else
            {
                result.Success = true;
                result.Data = new List<ReportItem>();

                List<ReportItem> waitingRequests = GetReportItemsFromReportDetailItems(reportResponse.Data.ReportDetailItemContainer.WaitingNumberReportDetailItems, MessageStatusEnum.WaitingOnHost);
                List<ReportItem> successfulRequests = GetReportItemsFromReportDetailItems(reportResponse.Data.ReportDetailItemContainer.SentNumberReportDetailItems, MessageStatusEnum.Success);
                List<ReportItem> failedRequests = GetReportItemsFromReportDetailItems(reportResponse.Data.ReportDetailItemContainer.FailedNumberReportDetailItems, MessageStatusEnum.Failed);

                if (waitingRequests != null && waitingRequests.Any())
                {
                    result.Data.AddRange(waitingRequests);
                }

                if (successfulRequests != null && successfulRequests.Any())
                {
                    result.Data.AddRange(successfulRequests);
                }

                if (failedRequests != null && failedRequests.Any())
                {
                    result.Data.AddRange(failedRequests);
                }

                return result;
            }

            return result;
        }

        #region Helpers

        private T Post<T>(string url, object payload) where T : BaseResponse, new()
        {
            T defaultResponse;

            try
            {
                string jsonString = _serializer.Serialize(payload);
                byte[] byteArray = Encoding.UTF8.GetBytes(jsonString);
                string base64String = Convert.ToBase64String(byteArray);
                string requestString = String.Concat("data=", base64String);
                byteArray = Encoding.UTF8.GetBytes(requestString);

                WebRequest request = WebRequest.Create(url);
                request.Timeout = (int)_timeOut.TotalMilliseconds;
                request.ContentType = "application/x-www-form-urlencoded";
                request.Method = "POST";
                request.ContentLength = byteArray.Length;

                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(byteArray, 0, byteArray.Length);
                }

                WebResponse response = request.GetResponse();

                using (Stream responseStream = response.GetResponseStream())
                {
                    if (responseStream != null && responseStream != Stream.Null)
                    {
                        using (StreamReader sr = new StreamReader(responseStream))
                        {
                            string responseString = sr.ReadToEnd().Trim();
                            byteArray = Convert.FromBase64String(responseString);
                            responseString = Encoding.UTF8.GetString(byteArray);
                            defaultResponse = _serializer.Deserialize<T>(responseString);
                            defaultResponse.ResponseString = responseString;
                            return defaultResponse;
                        }
                    }
                }

                defaultResponse = new T()
                {
                    Success = false,
                    ErrorMessage = "Cannot get any response from service."
                };
            }
            catch (Exception ex)
            {
                defaultResponse = new T()
                {
                    Success = false,
                    ErrorMessage = ex.Message
                };
            }

            return defaultResponse;
        }

        private List<Message> GetValidMessages(List<Message> messages)
        {
            List<Message> results = new List<Message>();

            if (messages != null && messages.Any())
            {
                foreach (Message message in messages)
                {
                    if (message != null && String.IsNullOrEmpty(message.Text) && message.Numbers != null && message.Numbers.Any())
                    {
                        Message newMessage = new Message()
                        {
                            Text = message.Text
                        };

                        List<string> numbers = message.Numbers.Where(n => _phoneNumberValidator.IsValid(n)).ToList();

                        if (numbers.Any())
                        {
                            newMessage.Numbers = numbers;
                        }

                        results.Add(newMessage);
                    }
                }
            }

            return results;
        }

        private List<ReportItem> GetReportItemsFromReportDetailItems(List<ReportDetailItem> reportDetailItems, MessageStatusEnum status)
        {
            if (reportDetailItems == null || !reportDetailItems.Any())
            {
                return null;
            }

            List<ReportItem> results = new List<ReportItem>();

            foreach (ReportDetailItem reportDetailItem in reportDetailItems)
            {
                if (reportDetailItem == null)
                {
                    continue;
                }

                ReportItem item = new ReportItem();

                item.DeliveredOn = GetDeliveryTimeFromReportDetailItem(reportDetailItem);
                item.PhoneNumber = reportDetailItem.Number;
                item.Status = status;
                item.Operator = GetOperatorFromReportDetailItem(reportDetailItem);

                if (item.Status == MessageStatusEnum.Failed)
                {
                    item.FailReason = GetFailReasonFromReportDetailItem(reportDetailItem);
                }

                results.Add(item);
            }

            return results;
        }

        private FailReasonEnum? GetFailReasonFromReportDetailItem(ReportDetailItem reportDetailItem)
        {
            if (reportDetailItem == null)
            {
                return null;
            }

            FailReasonEnum result;

            switch (reportDetailItem.FailReason)
            {
                case "cannot_deliver":
                    result = FailReasonEnum.Unknown;
                    break;
                case "memory_capacity_exceeded":
                    result = FailReasonEnum.MemoryCapacityExceeded;
                    break;
                case "foreign_operator":
                    result = FailReasonEnum.UnsupportedOperator;
                    break;
                case "foreign_country":
                    result = FailReasonEnum.UnsupportedCountry;
                    break;
                case "number_not_used":
                    result = FailReasonEnum.NumberNotInUse;
                    break;
                case "server_error":
                    result = FailReasonEnum.InternalServerError;
                    break;
                case "phone_closed":
                    result = FailReasonEnum.CellPhoneOutOfGrid;
                    break;
                default:
                    result = FailReasonEnum.Unknown;
                    break;
            }

            return result;
        }

        private OperatorEnum GetOperatorFromReportDetailItem(ReportDetailItem reportDetailItem)
        {
            if (String.IsNullOrEmpty(reportDetailItem.Operator))
            {
                return OperatorEnum.Unknown;
            }

            if (reportDetailItem.Operator.ToLower() == "avea")
            {
                return OperatorEnum.Avea;
            }

            if (reportDetailItem.Operator.ToLower() == "turkcell")
            {
                return OperatorEnum.Turkcell;
            }

            if (reportDetailItem.Operator.ToLower() == "vodafone")
            {
                return OperatorEnum.Vodafone;
            }

            return OperatorEnum.Unknown;
        }

        private DateTime? GetDeliveryTimeFromReportDetailItem(ReportDetailItem reportDetailItem)
        {
            if (String.IsNullOrEmpty(reportDetailItem.CompletedOn))
            {
                return null;
            }

            DateTime result;

            // UTC +2
            if (DateTime.TryParseExact(reportDetailItem.CompletedOn, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out result))
            {
                return result;
            }

            return null;
        }

        private ErrorCodeEnum GetErrorCode(SmsResponse smsResponse)
        {
            // TODO. We don't know error codes yet.
            if (smsResponse.ErrorMessage == "")
            {
                return ErrorCodeEnum.InsufficientCredits;
            }

            return ErrorCodeEnum.None;
        }

        #endregion
    }
}