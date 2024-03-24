using Microsoft.AspNetCore.Mvc;
using TransactionApi.Application.Services.Interface;
using TransactionApi.Domain.DTOs;
using TransactionApi.Web.Attribute;
using TransactionApi.Web.Helpers;
using Task = DocumentFormat.OpenXml.Office2021.DocumentTasks.Task;

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
    /// <response code="200">The Excel file is returned to the frontend.</response>
    /// <response code="404">Not found data in Db</response>
    /// GET: api/Transaction/ExportTransactionInExel
    [HttpGet]
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(ErrorResponse),404)]
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
    /// <response code="200">The List with Transactions is returned to the frontend.</response>
    /// /// <response code="404">Not found data in Db</response>
    /// GET: api/Transaction/GetTransaction
    [HttpGet]
    [TimeZoneHeader]
    [ProducesResponseType(typeof(List<TransactionResponse>),200)]
    [ProducesResponseType(typeof(ErrorResponse), 404)]
    public async Task<IActionResult> GetTransaction(int day, int month, int year,  string? timeZone)
    {
        var result = await _transactionService.GetTransactionByDateAndTimeZone(day, month, year, timeZone);
        return this.GetResponse(result);
    }

    /// <summary>
    /// Endpoint returns a list of transactions by time zon, and date range.
    /// </summary>
    /// <remarks>
    /// In header add time zon current users, and in query add time zon client if you need.<br/>
    /// If you add timeZone in query time zone from header will be ignore.
    /// The date in the request must be was in UNIX Time stamp in second and mandatory in UTC.
    /// </remarks>
    /// <returns>List Transaction</returns>
    /// <response code="200">The List with Transactions is returned to the frontend.</response>
    /// /// <response code="404">Not found data in Db</response>
    /// GET: api/Transaction/GetTransactionByDateRange
    [HttpGet]
    [TimeZoneHeader]
    [ProducesResponseType(typeof(List<TransactionResponse>),200)]
    [ProducesResponseType(typeof(ErrorResponse), 404)]
    public async Task<IActionResult> GetTransactionByDateRange(long dateFrom, long dateTo, string? timeZone)
    {
        var result = await _transactionService.GetTransactionByDateRange(dateFrom, dateTo, timeZone);
        return this.GetResponse(result);
    }
    
    /// <summary>
    /// Upload transaction data in csv
    /// </summary>
    /// <remarks>
    ///Upload Csv file with: transaction_id, name, email, amount, transaction_date, client_location.
    /// </remarks>
    /// <response code="204">Data add in Db</response>
    /// <response code="400">Error! Failed reading file or file Empty</response>
    /// Post: api/Transaction/UploadCsv
    [HttpPost]
    [ProducesResponseType(204)]
    [ProducesResponseType(typeof(ErrorResponse),400)]
    public async Task<IActionResult> UploadCsv(IFormFile file)
    {
        var result = await _transactionService.AddTransactionsFromCsvFile(file);
        return this.GetResponse(result);
    }
}