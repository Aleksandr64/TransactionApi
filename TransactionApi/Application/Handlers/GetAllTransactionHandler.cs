using Dapper;
using MediatR;
using TransactionApi.Application.Queries;
using TransactionApi.Database;
using TransactionApi.Domain.DTOs;
using TransactionApi.Domain.Model;

namespace TransactionApi.Application.Handlers;

public class GetAllTransactionHandler : IRequestHandler<GetAllTransactionQuery, IEnumerable<Transaction>>
{
    private readonly DapperContext _context;

    public GetAllTransactionHandler(DapperContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<Transaction>> Handle(GetAllTransactionQuery request, CancellationToken cancellationToken)
    {
        string sql = @"SELECT 
                            TransactionId, 
                            Name, 
                            Email, 
                            Amount, 
                            TransactionDate, 
                            TimeZone 
                        FROM Transactions";

        using (var connection = _context.CreateConnection())
        {
            return await connection.QueryAsync<Transaction>(sql);
        }
    }
}