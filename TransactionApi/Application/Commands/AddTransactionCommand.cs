using MediatR;
using TransactionApi.Domain.DTOs;
using TransactionApi.Domain.Model;

namespace TransactionApi.Application.Commands;

public record AddTransactionCommand(TransactionCSVRequest Transaction) : IRequest;