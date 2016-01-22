using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stark.Integration.SmsVitrini.Models;

namespace Stark.Integration.SmsVitrini.IntegrationTests
{
    [TestClass]
    public class IntegrationTest
    {
        private const string UserName = "";
        private const string Password = "";

        [TestMethod]
        public void SmsSendTest()
        {
            SmsClient client = new SmsClient(UserName, Password);
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
            SmsClient client = new SmsClient(UserName, Password);
            ServiceResult<CustomerDetail> result = client.GetCustomerDetails();
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
            Assert.IsNotNull(result.Data);
        }
    }
}
