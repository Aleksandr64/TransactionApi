using TransactionApi.Domain.DTOs;

namespace TransactionApi.Domain.ResultModels;

public class NotFoundResult<T> : Result<T>
{
    private readonly List<string> _error = new List<string>();
    
    public NotFoundResult(string error)
    {
        _error.Add(error);
    }
    public NotFoundResult(List<string> errors)
    {
        errors.ForEach(error => _error.Add(error));
    }

    public override ResultTypesEnum ResultType => ResultTypesEnum.BadRequest;

    public override ErrorResponse Errors => new ErrorResponse()
    {
        Status = 404,
        Message = "Not Found",
        Errors = _error
    };

    public override T Data => default!;
}