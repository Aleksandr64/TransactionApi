using MediatR;
using TransactionApi.Domain.Model;

namespace TransactionApi.Application.Queries;

public record class GetTransactionByDataQuery(int? Year, int? Month, int? TimeZoneOffsetInMinutes) : IRequest<IEnumerable<Transaction>>;