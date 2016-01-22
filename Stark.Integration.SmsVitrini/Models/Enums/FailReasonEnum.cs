namespace Stark.Integration.SmsVitrini.Models.Enums
{
    public enum FailReasonEnum
    {
        Unknown = 0,

        MemoryCapacityExceeded = 1,

        UnsupportedOperator = 2,

        UnsupportedCountry = 3,

        NumberNotInUse = 4,

        InternalServerError = 5,

        CellPhoneOutOfGrid = 6
    }
}