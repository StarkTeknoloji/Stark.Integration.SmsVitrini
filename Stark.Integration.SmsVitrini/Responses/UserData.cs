using System.Collections.Generic;

namespace Stark.Integration.SmsVitrini.Responses
{
    public class UserData
    {
        public string musteriid { get; set; }

        public string bayiid { get; set; }

        public string musterikodu { get; set; }

        public string yetkiliadsoyad { get; set; }

        public string firma { get; set; }

        public string orjinli { get; set; }

        public string sistem_kredi { get; set; }

        public List<string> basliklar { get; set; }
    }
}