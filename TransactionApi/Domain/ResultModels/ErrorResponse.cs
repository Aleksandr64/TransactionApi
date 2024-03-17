namespace TransactionApi.Domain.DTOs;

public class ErrorResponse
{
    public int Status { get; set; }
    public string Message { get; set; }
    public List<string> Errors { get; set; }
}