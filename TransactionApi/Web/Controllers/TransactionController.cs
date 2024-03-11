using System.Globalization;
using System.Text.RegularExpressions;
using GeoTimeZone;
using Microsoft.AspNetCore.Mvc;
using TimeZoneConverter;
using TransactionApi.Application.Services.Interface;
using TransactionApi.Domain.DTOs;
using TransactionApi.Web.Helpers;

namespace TransactionApi.Web.Controllers;

public class TransactionController : BaseApiController
{
    private readonly ITransactionService _transactionService;

    public TransactionController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    [HttpGet]
    public async Task<ActionResult> ExportTransactionInExel()
    {
        var file = await _transactionService.ExportTransactionInExel();
        return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Transaction.xlsx");
    }
    
    [HttpPost]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        var result = await _transactionService.AddCsvFile(file);
        return this.GetResponse(result);
    }
    
    /*[HttpPost]
    public IActionResult UploadCsvLibrary(IFormFile file)
    {
        var csvHelper = new CSVHelper();
        var list = csvHelper.GetCsvData(file);
        return Ok(list);
    }*/
    
}