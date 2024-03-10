using System.ComponentModel.DataAnnotations;

namespace TransactionApi.Domain.Model;

public class Transaction
{
    [Key]
    public string TransactionId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public decimal Amount { get; set; }
    public DateTimeOffset TransactionDate { get; set; }
    public string ClientLocation { get; set; }
}