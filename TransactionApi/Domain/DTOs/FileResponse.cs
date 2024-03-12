namespace TransactionApi.Domain.DTOs;

public class FileResponse
{
    public byte[] Content { get; set; }
    public string ContentType { get; set; }
    public string FileName { get; set; }
}