using System.Data;
using System.Text.RegularExpressions;
using ClosedXML.Excel;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using TransactionApi.Application.Commands;
using TransactionApi.Application.Helper;
using TransactionApi.Application.Mapper;
using TransactionApi.Application.Queries;
using TransactionApi.Application.Services.Interface;
using TransactionApi.Application.Validations;
using TransactionApi.Domain.DTOs;
using TransactionApi.Domain.ResultModels;

namespace TransactionApi.Application.Services;

public class TransactionService : ITransactionService
{
    private readonly IMediator _mediator;

    public TransactionService(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    public async Task<Result<string>> AddTransactionsFromCsvFile(IFormFile file)
    {
        var transactions = new List<TransactionCSVRequest>();
        using var reader = new StreamReader(file.OpenReadStream());
        await reader.ReadLineAsync();
        while (await reader.ReadLineAsync() is { } line)
        {
            string pattern = @",(?=(?:[^""]*""[^""]*"")*[^""]*$)";
            
            var fields = Regex.Split(line, pattern);
            var entity = fields.MapTransactionFromCsv();
            
            var validationResult = await new AddTransactionValidator().ValidateAsync(entity);
            if (!validationResult.IsValid)
            {
                continue;
            }
            
            entity.TransactionDate = TimeZoneHelper.ConvertTransactionTimeByLocation(entity.ClientLocation, entity.TransactionDate);
            
            transactions.Add(entity);
        }
        if (transactions.IsNullOrEmpty())
        {
            return new BadRequestResult<string>("Error! Failed reading file or file Empty");
        }

        foreach (var transaction in transactions)
        {
            var result = await _mediator.Send(new GetTransactionByIdQuery(transaction.TransactionId));
            if (result == null)
            {
                await _mediator.Send(new AddTransactionCommand(transaction));
            }
            else
            {
                await _mediator.Send(new UpdateTransactionCommand(transaction));
            }
        }
        return new SuccessResult<string>(null);
    }

    /// <inheritdoc />
    public async Task<Result<FileResponse>> ExportTransactionsInExel()
    {
        var result = await _mediator.Send(new GetAllTransactionQuery());

        if (result.IsNullOrEmpty())
        {
            return new NotFoundResult<FileResponse>("Not Found Data in Db");
        }

        var dataTable = new DataTable();
        dataTable.Columns.AddRange(new DataColumn[]
        {
            new DataColumn("Name"),
            new DataColumn("Email"),
            new DataColumn("Amount"),
            new DataColumn("TransactionDate")
        });

        foreach (var item in result)
        {
            dataTable.Rows.Add(item.Name, item.Email, item.Amount.ToString("C", new System.Globalization.CultureInfo("en-US")), item.TransactionDate.DateTime);
        }

        var fileResponse = new FileResponse
        {
            ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            FileName = "Transaction.xlsx"
        };

        using (var wb = new XLWorkbook())
        {
            var ws = wb.Worksheets.Add(dataTable, "Transaction");

            using (var stream = new MemoryStream())
            {
                wb.SaveAs(stream);
                fileResponse.Content = stream.ToArray();
            }
        }
        return new FileResult<FileResponse>(fileResponse);
    }

    /// <inheritdoc />
    public async Task<Result<IEnumerable<TransactionResponse>>> GetTransactionByDateAndTimeZone(int day, int month, int year,  string? timeZone)
    {
        int? offsetMinutes = TimeZoneHelper.GetTimeZoneOffsetMinutes(timeZone);
        
        var result = await _mediator.Send(new GetTransactionByDateQuery(day, month, year, offsetMinutes));

        if (!result.IsNullOrEmpty())
        {
            return new NotFoundResult<IEnumerable<TransactionResponse>>("Not Found Data in Db");
        }
        
        var transactionResponse = new List<TransactionResponse>();
        foreach (var item in result)
        {
            transactionResponse.Add(item.MapTransactionToResponse());
        }

        return new SuccessResult<IEnumerable<TransactionResponse>>(transactionResponse);
    }
}