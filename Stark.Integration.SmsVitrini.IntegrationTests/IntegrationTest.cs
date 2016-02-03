using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stark.Integration.SmsVitrini.Models;

namespace Stark.Integration.SmsVitrini.IntegrationTests
{
    [TestClass]
    public class IntegrationTest
    {
        private const string UserName = "username";
        private const string Password = "password";

        [TestMethod]
        public void SmsSendTest()
        {
            SmsVitriniClient client = new SmsVitriniClient(UserName, Password);
            ServiceResult<MessageResponse> result = client.Send("CAGRI SMS", new Message() { Numbers = new List<string>() { "5555555555" }, Text = "Sample string 1." });
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
            Assert.IsNotNull(result.Data);
        }

        [TestMethod]
        public void MultiSmsSendTest()
        {
            SmsVitriniClient client = new SmsVitriniClient(UserName, Password);
            ServiceResult<MessageResponse> result = client.Send("MODAGRAMIT", new List<Message>()
            {
                new Message() { Numbers = new List<string>() {"5542346742"}, Text = "Sample string 1."}
            });
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
            Assert.IsNotNull(result.Data);
        }

        [TestMethod]
        public void GetCustomerDetailsTest()
        {
            SmsVitriniClient client = new SmsVitriniClient(UserName, Password);
            ServiceResult<CustomerDetail> result = client.GetCustomerDetails();
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
            Assert.IsNotNull(result.Data);
        }

        [TestMethod]
        public void GetReportsTest()
        {
            SmsVitriniClient client = new SmsVitriniClient(UserName, Password);
            ServiceResult<List<ReportItem>> result = client.GetReports("28218827");
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
            Assert.IsNotNull(result.Data);
        }
    }
}
