using MediatR;
using TransactionApi.Domain.DTOs;

namespace TransactionApi.Application.Queries;

public record class GetTransactionByDateRangeQuery(string DateFrom, string DateTo, int TimeZone) : IRequest<IEnumerable<TransactionDTO>>;