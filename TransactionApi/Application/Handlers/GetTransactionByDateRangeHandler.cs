using Dapper;
using MediatR;
using TransactionApi.Application.Queries;
using TransactionApi.Database;
using TransactionApi.Domain.DTOs;
using TransactionApi.Domain.Model;

namespace TransactionApi.Application.Handlers;

public class GetTransactionByDateRangeHandler : IRequestHandler<GetTransactionByDateRangeQuery, IEnumerable<Transaction>>
{
    private readonly DapperContext _context;

    public GetTransactionByDateRangeHandler(DapperContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<Transaction>> Handle(GetTransactionByDateRangeQuery request, CancellationToken cancellationToken)
    {
        string sql = @"SELECT 
                        TransactionId, 
                        Name, 
                        Email, 
                        Amount, 
                        CAST(TransactionDate AT TIME ZONE 'UTC' AT TIME ZONE @TimeZoneFilter AS datetime) AS TransactionDate,
                        TimeZone
                       FROM Transactions
                       WHERE CAST(TransactionDate AT TIME ZONE 'UTC' AT TIME ZONE @TimeZoneFilter AS datetime) >= @DateFrom  
                        AND CAST(TransactionDate AT TIME ZONE 'UTC' AT TIME ZONE @TimeZoneFilter AS datetime)  <= @DateTo;";

        using (var connection = _context.CreateConnection())
        {
            return await connection.QueryAsync<Transaction>(sql,
                new
                {
                    DateFrom = request.DateFrom,
                    DateTo = request.DateTo,
                    TimeZoneFilter = request.TimeZone
                });
        }
    }
}