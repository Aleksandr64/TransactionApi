using TransactionApi.Domain.ResultModels;

namespace TransactionApi.Application.Services.Interface;

public interface ITransactionService
{
    public Task<Result<string>> AddCsvFile(IFormFile file);
    public Task<byte[]> ExportTransactionInExel();
}