using Dapper;
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
        string sql = @"SELECT 
                            TransactionId, 
                            Name, 
                            Email, 
                            Amount, 
                            CAST(TransactionDate AT TIME ZONE 'UTC' AT TIME ZONE @TimeZoneFilter AS datetime) AS TransactionDate, 
                            TimeZone 
                        FROM Transactions
                        WHERE (@YearFilter = 0 OR YEAR(CAST(TransactionDate AT TIME ZONE 'UTC' AT TIME ZONE @TimeZoneFilter AS datetime)) = @YearFilter)
                            AND (@MonthFilter = 0 OR MONTH(CAST(TransactionDate AT TIME ZONE 'UTC' AT TIME ZONE @TimeZoneFilter AS datetime)) = @MonthFilter)
                            AND (@DayFilter = 0 OR DAY(CAST(TransactionDate AT TIME ZONE 'UTC' AT TIME ZONE @TimeZoneFilter AS datetime)) = @DayFilter);";

        using (var connection = _context.CreateConnection())
        {
            return await connection.QueryAsync<Transaction>(sql,
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