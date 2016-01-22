namespace Stark.Integration.SmsVitrini
{
    public interface IPhoneNumberValidator
    {
        bool IsValid(string number);
    }
}