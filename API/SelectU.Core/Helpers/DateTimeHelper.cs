namespace SelectU.Core.Helpers
{
    public static class DateTimeHelper
    {
        public static DateTimeOffset DateTimeWithAusEasternStandardTime(DateTimeOffset dateTime)
        {
            var timezone = TimeZoneInfo.FindSystemTimeZoneById("AUS Eastern Standard Time");
            return TimeZoneInfo.ConvertTime(dateTime, timezone);
        }
    }
}
