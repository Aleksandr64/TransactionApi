using MediatR;
using TransactionApi.Domain.Model;

namespace TransactionApi.Application.Commands;

public record AddTransactionCommand(Transaction Transaction) : IRequest;