using MediatR;
using TransactionApi.Domain.DTOs;
using TransactionApi.Domain.Model;

namespace TransactionApi.Application.Queries;

public record class GetTransactionByDateQuery(int Day, int Month,int Year, string TimeZone) : IRequest<IEnumerable<Transaction>>;