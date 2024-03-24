using GeoTimeZone;
using TimeZoneConverter;
using TransactionApi.Domain.DTOs;

namespace TransactionApi.Application.Helper;

public static class TimeZoneHelper
{
    public static int GetTimeZoneOffsetMinutes(string timeZone)
    {
        TimeZoneInfo tz = TZConvert.GetTimeZoneInfo(timeZone);
        return (int)tz.BaseUtcOffset.TotalMinutes;
    }

    public static string GetTimeZoneByLocation(string clientLocation)
    {
        var location = clientLocation.Split(',');
        string tz = TimeZoneLookup.GetTimeZone(double.Parse(location[0]), double.Parse(location[1])).Result;
        return tz;
    }
    public static DateTime ConvertTransactionTimeByTimeZoneToUTC(DateTime dateTime ,string timeZone)
    {
        TimeZoneInfo tz = TZConvert.GetTimeZoneInfo(timeZone);
        return TimeZoneInfo.ConvertTimeToUtc(dateTime, tz);
    }

    public static DateTimeOffset ConverTransactionTimeByTimeZone(DateTimeOffset transactionDateTime, string tz)
    {
        TimeZoneInfo timeZone = TZConvert.GetTimeZoneInfo(tz);
        return TimeZoneInfo.ConvertTime(transactionDateTime, timeZone);
    }

    public static string ConvertUnixTimeStampToDateToString(long unixDateTime)
    {
        DateTimeOffset dateTime = DateTimeOffset.FromUnixTimeSeconds(unixDateTime).UtcDateTime;
        return dateTime.ToString("yyyy-MM-dd");
    }
}