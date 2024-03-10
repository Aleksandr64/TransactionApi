﻿using System.Globalization;
using TransactionApi.Domain.Model;

namespace TransactionApi.Application.Mapper;

public static class TransactionMapper
{
    public static Transaction MapTransactionFromCsv(this string[] values)
    {
        return new Transaction() 
        {
            TransactionId = values[0],
            Name = values[1],
            Email = values[2],
            Amount = decimal.Parse(values[3], NumberStyles.Currency, CultureInfo.GetCultureInfo("en-US")), 
            TransactionDate = DateTimeOffset.Parse(values[4], CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal),
            ClientLocation = values[5].Trim('"')
        };
    }
}