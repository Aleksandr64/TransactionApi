using MediatR;
using TransactionApi.Domain.Model;

namespace TransactionApi.Application.Queries;

public record class GetTransactionByDateQuery(int Day, int Month,int Year, int? TimeZoneOffsetInMinutes) : IRequest<IEnumerable<Transaction>>;