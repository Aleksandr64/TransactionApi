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
    /// This endpoint allows exporting transactions to Excel format.
    /// </remarks>
    /// <returns>
    /// Excel file containing transaction information.
    /// </returns>
    /// GET: api/Transaction/ExportTransactionInExel
    [HttpGet]
    public async Task<IActionResult> ExportTransactionInExel()
    {
        var result = await _transactionService.ExportTransactionInExel();
        return this.GetResponse(result);
    }

    /// <summary>
    /// This endpoint returns a list of transactions in the endpoint by time zon, and date
    /// </summary>
    /// <remarks>
    /// In endpoint you can in header add time zon current users, and  in url add time zon client. 
    /// Also you can add Year, Month and Date in property to filter transaction by date.
    /// </remarks>
    /// <param name="year"></param>
    /// <param name="month"></param>
    /// <param name="timeZone"></param>
    /// <returns></returns>
    [HttpGet]
    [TimeZoneHeader]
    public async Task<IActionResult> GetTransaction(int year, int month, string? timeZone)
    {
        var result = await _transactionService.GetTransactionByData(year, month, timeZone);
        return this.GetResponse(result);
    }
    
    /// <summary>
    /// Upload transaction data in csv
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    /// <response code="204">Returns the newly created item</response>
    [HttpPost]
    [ProducesResponseType(201)]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        var result = await _transactionService.AddCsvFile(file);
        return this.GetResponse(result);
    }
}