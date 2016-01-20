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
            client.Send("CAGRISMS", new List<Message>() {new Message() { Numbers = new List<string>() {"5542346742"}, Text = "Test Mesajıdır."}});
        }
    }
}
