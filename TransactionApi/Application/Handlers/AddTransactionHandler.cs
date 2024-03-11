using Dapper;
using MediatR;
using TransactionApi.Application.Commands;
using TransactionApi.Database;

namespace TransactionApi.Application.Handlers;

public class AddTransactionHandler : IRequestHandler<AddTransactionCommand>
{
    private readonly DapperContext _context;

    public AddTransactionHandler(DapperContext context)
    {
        _context = context;
    }
    public async Task Handle(AddTransactionCommand request, CancellationToken cancellationToken)
    {
        string sql = @"INSERT INTO Transactions (TransactionId, Name, Email, Amount, TransactionDate, ClientLocation) 
                       VALUES (@TransactionId, @Name, @Email, @Amount, @TransactionDate, @ClientLocation)";

        using (var connection = _context.CreateConnection())
        {
            await connection.ExecuteAsync(sql, request.Transaction);
        }
    }
}