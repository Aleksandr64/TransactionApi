using CsvHelper.Configuration.Attributes;

namespace TransactionApi.Domain.DTOs;

public class TransactionRequest
{
    public string TransactionId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public decimal Amount { get; set; }
    public DateTimeOffset TransactionDate { get; set; }
    public string ClientLocation { get; set; }
    
}