using Dapper;
using MediatR;
using TransactionApi.Application.Queries;
using TransactionApi.Database;
using TransactionApi.Domain.DTOs;

namespace TransactionApi.Application.Handlers;

public class GetTransactionByDateRangeHandler : IRequestHandler<GetTransactionByDateRangeQuery, IEnumerable<TransactionDTO>>
{
    private readonly DapperContext _context;

    public GetTransactionByDateRangeHandler(DapperContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<TransactionDTO>> Handle(GetTransactionByDateRangeQuery request, CancellationToken cancellationToken)
    {
        string sql = @"SELECT 
                            TransactionId, 
                            Name, 
                            Email, 
                            Amount, 
                            dbo.ConvertTimeStampToDateWithOffset(TransactionDate, @TimeZoneFilter) AS TransactionDate, 
                            TimeZone 
                        FROM Transactions
                        WHERE (CONVERT(DATE, dbo.ConvertTimeStampToDateWithOffset(TransactionDate, @TimeZoneFilter)) >= @DateFrom)
                            AND (CONVERT(DATE, dbo.ConvertTimeStampToDateWithOffset(TransactionDate, @TimeZoneFilter)) <= @DateTo)";

        using (var connection = _context.CreateConnection())
        {
            return await connection.QueryAsync<TransactionDTO>(sql,
                new
                {
                    DateFrom = request.DateFrom,
                    DateTo = request.DateTo,
                    TimeZoneFilter = request.TimeZone
                });
        }
    }
}