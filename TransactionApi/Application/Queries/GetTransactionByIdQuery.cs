using MediatR;
using TransactionApi.Domain.Model;

namespace TransactionApi.Application.Queries;

public record GetTransactionByIdQuery(string Id) : IRequest<Transaction>;