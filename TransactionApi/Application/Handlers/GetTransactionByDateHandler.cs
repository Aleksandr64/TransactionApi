using Dapper;
using DocumentFormat.OpenXml.Bibliography;
using MediatR;
using TransactionApi.Application.Queries;
using TransactionApi.Database;
using TransactionApi.Domain.Model;

namespace TransactionApi.Application.Handlers;

public class GetTransactionByDateHandler : IRequestHandler<GetTransactionByDateQuery, IEnumerable<Transaction>>
{
    private readonly DapperContext _context;

    public GetTransactionByDateHandler(DapperContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<Transaction>> Handle(GetTransactionByDateQuery request, CancellationToken cancellationToken)
    {
        string sql = @"SELECT *
                    FROM Transactions
                    WHERE (@YearFilter = 0 OR YEAR(TransactionDate) = @YearFilter)
                        AND (@MonthFilter = 0 OR MONTH(TransactionDate) = @MonthFilter)
                        AND (@DayFilter = 0 OR DAY(TransactionDate) = @DayFilter)
                        AND (@TimeZoneOffsetFilter IS NULL OR DATEPART(TZOFFSET, TransactionDate) = @TimeZoneOffsetFilter)";

        using (var connection = _context.CreateConnection())
        {
            return await connection.QueryAsync<Transaction>(sql,
                new
                {
                    DayFilter = request.Day,
                    MonthFilter = request.Month, 
                    YearFilter = request.Year,
                    TimeZoneOffsetFilter = request.TimeZoneOffsetInMinutes
                });
        }
    }
}