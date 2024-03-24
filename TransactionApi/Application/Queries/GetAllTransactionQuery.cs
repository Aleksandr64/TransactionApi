using MediatR;
using TransactionApi.Domain.DTOs;
using TransactionApi.Domain.Model;

namespace TransactionApi.Application.Queries;

public record GetAllTransactionQuery : IRequest<IEnumerable<TransactionDTO>>;