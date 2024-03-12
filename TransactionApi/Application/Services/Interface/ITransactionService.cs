using TransactionApi.Domain.DTOs;
using TransactionApi.Domain.ResultModels;
using Task = DocumentFormat.OpenXml.Office2021.DocumentTasks.Task;

namespace TransactionApi.Application.Services.Interface;

public interface ITransactionService
{
    public Task<Result<string>> AddCsvFile(IFormFile file);
    public Task<Result<FileResponse>> ExportTransactionInExel();
    public Task<Result<IEnumerable<TransactionResponse>>> GetTransactionByData(int? year, int? month, int? timeZoneOffsetInMinutes);
}