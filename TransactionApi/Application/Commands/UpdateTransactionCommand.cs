using MediatR;
using TransactionApi.Domain.Model;

namespace TransactionApi.Application.Commands;

public record UpdateTransactionCommand(Transaction Transaction) : IRequest;