namespace TransactionApi.Domain.DTOs;

public class TransactionResponse
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Amount { get; set; }
    public DateTime TransactionDate { get; set; }
}