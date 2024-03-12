namespace TransactionApi.Domain.ResultModels;

public class FileResult<T> : Result<T>
{
    private readonly T _data;
    
    public FileResult(T data)
    {
        _data = data;
    }

    public override ResultTypesEnum ResultType => ResultTypesEnum.File;
    public override List<string> Errors => new List<string>();
    public override T Data => _data;
}