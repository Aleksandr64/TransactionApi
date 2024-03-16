using System.Globalization;
using TransactionApi.Domain.DTOs;
using TransactionApi.Domain.Model;

namespace TransactionApi.Application.Mapper;

public static class TransactionMapper
{
    public static TransactionCSVRequest MapTransactionFromCsv(this string[] values)
    {
        return new TransactionCSVRequest() 
        {
            TransactionId = values[0],
            Name = values[1],
            Email = values[2],
            Amount = decimal.Parse(values[3], NumberStyles.Currency, CultureInfo.GetCultureInfo("en-US")), 
            TransactionDate = DateTimeOffset.Parse(values[4], CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal),
            ClientLocation = values[5].Trim('"')
        };
    }

    public static TransactionResponse MapTransactionToResponse(this Transaction item)
    {
        return new TransactionResponse
        {
            Name = item.Name,
            Email = item.Email,
            Amount = item.Amount.ToString("C",new System.Globalization.CultureInfo("en-US")),
            TransactionDate = item.TransactionDate
        };
    }
}