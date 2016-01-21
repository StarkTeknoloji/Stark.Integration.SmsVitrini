using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stark.Integration.SmsVitrini.Models;

namespace Stark.Integration.SmsVitrini.IntegrationTests
{
    [TestClass]
    public class IntegrationTest
    {
        [TestMethod]
        public void SmsSendTest()
        {
            SmsClient client = new SmsClient("", "");
            client.Send("CAGRISMS", new List<Message>()
            {
                new Message() { Numbers = new List<string>() {"5542346742"}, Text = "5542346742"}
            });
        }

        [TestMethod]
        public void GetCustomerDetailsTest()
        {
            SmsClient client = new SmsClient("", "");
            ServiceResult<CustomerDetail> result = client.GetCustomerDetails();
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
            Assert.IsNotNull(result.Data);
        }
    }
}
