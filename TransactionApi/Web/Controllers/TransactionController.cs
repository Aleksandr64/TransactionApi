using Microsoft.AspNetCore.Mvc;
using TransactionApi.Application.Services.Interface;
using TransactionApi.Web.Attribute;
using TransactionApi.Web.Helpers;

namespace TransactionApi.Web.Controllers;

public class TransactionController : BaseApiController
{
    private readonly ITransactionService _transactionService;
    public TransactionController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    /// <summary>
    /// Exports transactions to Excel format.
    /// </summary>
    /// <remarks>
    /// Export all transaction in xlsx with column: <br/>
    /// Name, Address, Amount, DateTime
    /// </remarks>
    /// <returns>
    /// Excel file.
    /// </returns>
    /// GET: api/Transaction/ExportTransactionInExel
    [HttpGet]
    public async Task<IActionResult> ExportTransactionInExel()
    {
        var result = await _transactionService.ExportTransactionsInExel();
        return this.GetResponse(result);
    }

    /// <summary>
    /// Endpoint returns a list of transactions by time zon, and date.
    /// </summary>
    /// <remarks>
    /// In header add time zon current users, and in query add time zon client if you need.<br/>
    /// If you add timeZone in query time zone from header will be ignore.
    /// Year, Month and Date in query to filter transaction by date.
    /// </remarks>
    /// <returns>List Transaction</returns>
    /// GET: api/Transaction/GetTransaction
    [HttpGet]
    [TimeZoneHeader]
    public async Task<IActionResult> GetTransaction(int day, int month, int year,  string? timeZone)
    {
        var result = await _transactionService.GetTransactionByDateAndTimeZone(day, month, year, timeZone);
        return this.GetResponse(result);
    }
    
    /// <summary>
    /// Upload transaction data in csv
    /// </summary>
    /// <response code="204">All data add in Db</response>
    /// Post: api/Transaction/UploadCsv
    [HttpPost]
    [ProducesResponseType(204)]
    public async Task<IActionResult> UploadCsv(IFormFile file)
    {
        var result = await _transactionService.AddTransactionsFromCsvFile(file);
        return this.GetResponse(result);
    }
}