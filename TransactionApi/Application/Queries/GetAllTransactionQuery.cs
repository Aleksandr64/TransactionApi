using MediatR;

using TransactionApi.Domain.Model;

namespace TransactionApi.Application.Queries;

public record GetAllTransactionQuery : IRequest<IEnumerable<Transaction>>;