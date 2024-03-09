namespace TransactionApi.Domain.ResultModels;

public class SuccessResult<T> : Result<T>
{
    private readonly T _data;
    
    public SuccessResult(T data)
    {
        _data = data;
    }

    public override ResultTypesEnum ResultType => ResultTypesEnum.Success;
    public override List<string> Errors => new List<string>();
    public override T Data => _data;
}