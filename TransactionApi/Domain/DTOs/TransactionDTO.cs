namespace TransactionApi.Domain.DTOs;

public class TransactionDTO
{
    public string TransactionId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public decimal Amount { get; set; }
    public DateTimeOffset TransactionDate { get; set; }
    public string TimeZone { get; set; }
}