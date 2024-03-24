using MediatR;
using TransactionApi.Domain.DTOs;
using TransactionApi.Domain.Model;

namespace TransactionApi.Application.Queries;

public record GetTransactionByIdQuery(string Id) : IRequest<TransactionDTO>;