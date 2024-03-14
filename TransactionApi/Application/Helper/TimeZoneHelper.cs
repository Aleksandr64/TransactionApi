using GeoTimeZone;
using TimeZoneConverter;

namespace TransactionApi.Application.Helper;

public static class TimeZoneHelper
{
    public static int? GetTimeZoneOffsetMinutes(string timeZone)
    {
        int? offsetMinutes = null;
        if (!string.IsNullOrEmpty(timeZone))
        {
            TimeZoneInfo gmtTimeZone = TZConvert.GetTimeZoneInfo(timeZone);
            offsetMinutes = Convert.ToInt32(gmtTimeZone.BaseUtcOffset.TotalMinutes);
        }
        return offsetMinutes;
    }

    public static DateTimeOffset ConvertTransactionTimeByLocation(string clientLocation, DateTimeOffset transactionDate)
    {
        var location = clientLocation.Split(',');
        string tz = TimeZoneLookup.GetTimeZone(double.Parse(location[0]), double.Parse(location[1])).Result;
        TimeZoneInfo timeZone = TZConvert.GetTimeZoneInfo(tz);
        return TimeZoneInfo.ConvertTime(transactionDate, timeZone);
    }
}