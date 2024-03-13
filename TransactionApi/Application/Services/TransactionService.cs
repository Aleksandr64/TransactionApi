using System.Data;
using System.Text.RegularExpressions;
using ClosedXML.Excel;
using GeoTimeZone;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using TimeZoneConverter;
using TransactionApi.Application.Commands;
using TransactionApi.Application.Mapper;
using TransactionApi.Application.Queries;
using TransactionApi.Application.Services.Interface;
using TransactionApi.Application.Validations;
using TransactionApi.Domain.DTOs;
using TransactionApi.Domain.Model;
using TransactionApi.Domain.ResultModels;

namespace TransactionApi.Application.Services;

public class TransactionService : ITransactionService
{
    private readonly IMediator _mediator;

    public TransactionService(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <inheritdoc />
    public async Task<Result<string>> AddCsvFile(IFormFile file)
    {
        var transactions = new List<Transaction>();
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
                //TODO 207 Status code
                continue;
            }
            
            var location = entity.ClientLocation.Split(',');
            
            string tz = TimeZoneLookup.GetTimeZone(double.Parse(location[0]), double.Parse(location[1])).Result;
            TimeZoneInfo timeZone = TZConvert.GetTimeZoneInfo(tz);
            entity.TransactionDate = TimeZoneInfo.ConvertTime(entity.TransactionDate, timeZone);
            
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
    public async Task<Result<FileResponse>> ExportTransactionInExel()
    {
        var result = await _mediator.Send(new GetAllTransactionQuery());

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
            dataTable.Rows.Add(item.Name, item.Email, item.Amount, item.TransactionDate.DateTime);
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
    public async Task<Result<IEnumerable<TransactionResponse>>> GetTransactionByData(int? year, int? month, string? timeZone)
    {
        int? offsetMinutes = null;
        if (!timeZone.IsNullOrEmpty())
        {
            TimeZoneInfo gmtTimeZone = TZConvert.GetTimeZoneInfo(timeZone);
            offsetMinutes = (int)gmtTimeZone.BaseUtcOffset.TotalMinutes;
        }
        var result = await _mediator.Send(new GetTransactionByDataQuery(year, month, offsetMinutes));

        var transactionResponse = new List<TransactionResponse>();
        foreach (var item in result)
        {
            transactionResponse.Add(item.MapTransactionToResponse());
        }

        return new SuccessResult<IEnumerable<TransactionResponse>>(transactionResponse);
    }
}