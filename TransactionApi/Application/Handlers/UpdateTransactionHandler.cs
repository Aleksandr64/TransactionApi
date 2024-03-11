using Dapper;
using MediatR;
using TransactionApi.Application.Commands;
using TransactionApi.Database;

namespace TransactionApi.Application.Handlers;

public class UpdateTransactionHandler : IRequestHandler<UpdateTransactionCommand>
{
    private readonly DapperContext _context;

    public UpdateTransactionHandler(DapperContext context)
    {
        _context = context;
    }
    public async Task Handle(UpdateTransactionCommand request, CancellationToken cancellationToken)
    {
        string sql = @"UPDATE Transactions 
                       SET Name = @Name, 
                           Email = @Email, 
                           Amount = @Amount, 
                           TransactionDate = @TransactionDate, 
                           ClientLocation = @ClientLocation
                       WHERE TransactionId = @TransactionId";

        using (var connection = _context.CreateConnection())
        {
            await connection.ExecuteAsync(sql, request.Transaction);
        }
    }
}