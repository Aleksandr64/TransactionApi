using TransactionApi.Domain.DTOs;
using TransactionApi.Domain.ResultModels;
using Task = DocumentFormat.OpenXml.Office2021.DocumentTasks.Task;

namespace TransactionApi.Application.Services.Interface;

public interface ITransactionService
{
    public Task<Result<string>> AddTransactionsFromCsvFile(IFormFile file);
    public Task<Result<FileResponse>> ExportTransactionsInExel();
    public Task<Result<IEnumerable<TransactionResponse>>> GetTransactionByDateAndTimeZone(int day, int month, int year,  string? timeZone);
    public Task<Result<IEnumerable<TransactionResponse>>> GetTransactionByDateRange(string dateFrom, string dateTo, string? timeZone);
}