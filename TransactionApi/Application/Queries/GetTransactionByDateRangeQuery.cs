using MediatR;
using TransactionApi.Domain.DTOs;
using TransactionApi.Domain.Model;

namespace TransactionApi.Application.Queries;

public record class GetTransactionByDateRangeQuery(string DateFrom, string DateTo, string TimeZone) : IRequest<IEnumerable<Transaction>>;