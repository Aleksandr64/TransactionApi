using Dapper;
using DocumentFormat.OpenXml.Bibliography;
using MediatR;
using TransactionApi.Application.Queries;
using TransactionApi.Database;
using TransactionApi.Domain.Model;

namespace TransactionApi.Application.Handlers;

public class GetTransactionByDataHandler : IRequestHandler<GetTransactionByDataQuery, IEnumerable<Transaction>>
{
    private readonly DapperContext _context;

    public GetTransactionByDataHandler(DapperContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<Transaction>> Handle(GetTransactionByDataQuery request, CancellationToken cancellationToken)
    {
        string sql = @"SELECT *
                    FROM Transactions
                    WHERE (@YearFilter IS NULL OR YEAR(TransactionDate) = @YearFilter)
                        AND (@MonthFilter IS NULL OR MONTH(TransactionDate) = @MonthFilter)
                        AND (@TimeZoneOffsetFilter IS NULL OR DATEPART(TZOFFSET, TransactionDate) = @TimeZoneOffsetFilter)";

        using (var connection = _context.CreateConnection())
        {
            return await connection.QueryAsync<Transaction>(sql,
                new
                {
                    MonthFilter = request.Month, 
                    YearFilter = request.Year,
                    TimeZoneOffsetFilter = request.TimeZoneOffsetInMinutes
                });
        }
    }
}