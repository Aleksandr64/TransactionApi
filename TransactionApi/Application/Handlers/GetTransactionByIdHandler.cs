using Dapper;
using MediatR;
using TransactionApi.Application.Queries;
using TransactionApi.Database;
using TransactionApi.Domain.DTOs;
using TransactionApi.Domain.Model;

namespace TransactionApi.Application.Handlers;

public class GetTransactionByIdHandler : IRequestHandler<GetTransactionByIdQuery, TransactionDTO>
{
    private readonly DapperContext _context;

    public GetTransactionByIdHandler(DapperContext context)
    {
        _context = context;
    }
    
    public async Task<TransactionDTO> Handle(GetTransactionByIdQuery request, CancellationToken cancellationToken)
    {
        string sql = @"SELECT 
                            TransactionId, 
                            Name, 
                            Email, 
                            Amount, 
                            dbo.ConvertTimeStampToDateWithOffset(TransactionDate, 0) AS TransactionDate, 
                            TimeZone 
                        FROM Transactions
                        WHERE TransactionId = @TransactionId";

        using (var connection = _context.CreateConnection())
        {
            return await connection.QueryFirstOrDefaultAsync<TransactionDTO>(sql, new { TransactionId = request.Id });
        }
    }
}