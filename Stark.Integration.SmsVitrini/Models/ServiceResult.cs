using Stark.Integration.SmsVitrini.Models.Enums;

namespace Stark.Integration.SmsVitrini.Models
{
    public class ServiceResult
    {
        public bool Success { get; set; }
        
        public string Message { get; set; }

        public ErrorCodeEnum ErrorCode { get; set; }
    }

    public class ServiceResult<T> : ServiceResult
    {
        public T Data { get; set; }
    }
}
