using System.Globalization;
using System.Text.RegularExpressions;
using GeoTimeZone;
using TimeZoneConverter;
using TransactionApi.Application.Mapper;
using TransactionApi.Application.Services.Interface;
using TransactionApi.Domain.DTOs;
using TransactionApi.Domain.Model;
using TransactionApi.Domain.ResultModels;

namespace TransactionApi.Application.Services;

public class TransactionService : ITransactionService
{
    public async Task<Result<string>> AddCsvFile(IFormFile file)
    {
        var list = new List<Transaction>();
        using var reader = new StreamReader(file.OpenReadStream());
        await reader.ReadLineAsync();
        while (reader.ReadLine() is { } line)
        {
            string pattern = @",(?=(?:[^""]*""[^""]*"")*[^""]*$)";
            
            var fields = Regex.Split(line, pattern);
            
            var entity = fields.MapTransactionFromCsv();
            
            var location = entity.ClientLocation.Split(',');
            
            string tz = TimeZoneLookup.GetTimeZone(double.Parse(location[0]), double.Parse(location[1])).Result;
            
            TimeZoneInfo timeZone = TZConvert.GetTimeZoneInfo(tz);
            
            entity.TransactionDate = TimeZoneInfo.ConvertTime(entity.TransactionDate, timeZone);
            
            list.Add(entity);
        }

        return new SuccessResult<string>(null);
    }
}