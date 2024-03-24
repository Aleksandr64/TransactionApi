using Dapper;
using DocumentFormat.OpenXml.Bibliography;
using MediatR;
using TransactionApi.Application.Queries;
using TransactionApi.Database;
using TransactionApi.Domain.DTOs;
using TransactionApi.Domain.Model;

namespace TransactionApi.Application.Handlers;

public class GetTransactionByDateHandler : IRequestHandler<GetTransactionByDateQuery, IEnumerable<TransactionDTO>>
{
    private readonly DapperContext _context;

    public GetTransactionByDateHandler(DapperContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<TransactionDTO>> Handle(GetTransactionByDateQuery request, CancellationToken cancellationToken)
    {
        string sql = @"SELECT 
                            TransactionId, 
                            Name, 
                            Email, 
                            Amount, 
                            dbo.ConvertTimeStampToDateWithOffset(TransactionDate, @TimeZoneFilter) AS TransactionDate, 
                            TimeZone 
                        FROM Transactions
                        WHERE (@YearFilter = 0 OR YEAR(dbo.ConvertTimeStampToDateWithOffset(TransactionDate, @TimeZoneFilter)) = @YearFilter)
                            AND (@MonthFilter = 0 OR MONTH(dbo.ConvertTimeStampToDateWithOffset(TransactionDate, @TimeZoneFilter)) = @MonthFilter)
                            AND (@DayFilter = 0 OR DAY(dbo.ConvertTimeStampToDateWithOffset(TransactionDate, @TimeZoneFilter)) = @DayFilter)";

        using (var connection = _context.CreateConnection())
        {
            return await connection.QueryAsync<TransactionDTO>(sql,
                new
                {
                    DayFilter = request.Day,
                    MonthFilter = request.Month, 
                    YearFilter = request.Year,
                    TimeZoneFilter = request.TimeZone
                });
        }
    }
}