using Microsoft.AspNetCore.Mvc;
using TransactionApi.Application.Services.Interface;
using TransactionApi.Web.Attribute;
using TransactionApi.Web.Helpers;

namespace TransactionApi.Web.Controllers;

public class TransactionController : BaseApiController
{
    private readonly ITransactionService _transactionService;
    public TransactionController(ITransactionService transactionService, IHttpContextAccessor contextAccessor)
    {
        _transactionService = transactionService;
    }

    [HttpGet]
    public async Task<IActionResult> ExportTransactionInExel()
    {
        var result = await _transactionService.ExportTransactionInExel();
        return this.GetResponse(result);
    }

    [HttpGet]
    [TimeZoneHeader]
    public async Task<IActionResult> GetTransaction(int? year, int? month, int? timeZoneOffsetInMinutes)
    {
        var result = await _transactionService.GetTransactionByData(year, month, timeZoneOffsetInMinutes);
        return this.GetResponse(result);
    }
    
    [HttpPost]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        var result = await _transactionService.AddCsvFile(file);
        return this.GetResponse(result);
    }
}