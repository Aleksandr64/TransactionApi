using System.Globalization;
using System.Text.RegularExpressions;
using GeoTimeZone;
using TimeZoneConverter;
using TransactionApi.Domain.DTOs;

namespace TransactionApi.Application.Helper;

public static class TimeZoneHelper
{
    public static string GetTimeZoneStandartName(string tz)
    {
        return TZConvert.GetTimeZoneInfo(tz).Id;
    }

    public static string GetTimeZoneByLocation(string clientLocation)
    {
        var location = clientLocation.Split(',');
        string tz = TimeZoneLookup.GetTimeZone(double.Parse(location[0]), double.Parse(location[1])).Result;
        var test = TZConvert.GetTimeZoneInfo(tz);
        return test.Id;
    }
    public static DateTime ConvertTransactionTimeByTimeZoneToUTC(DateTime dateTime ,string tz)
    {
        TimeZoneInfo timeZone = TZConvert.GetTimeZoneInfo(tz);
        return TimeZoneInfo.ConvertTimeToUtc(dateTime, timeZone);
    }

    public static DateTime ConverTransactionTimeByTimeZone(DateTime transactionDateTime, string tz)
    {
        var dateTimeUtc = DateTime.SpecifyKind(transactionDateTime, DateTimeKind.Utc);
        TimeZoneInfo timeZone = TZConvert.GetTimeZoneInfo(tz);
        return TimeZoneInfo.ConvertTime(dateTimeUtc, timeZone);
    }

    public static bool CheckDateInString(string date)
    {
        return DateTime.TryParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out _);
    }
}