﻿using TransactionApi.Domain.DTOs;

namespace TransactionApi.Domain.ResultModels;

public class BadRequestResult<T> : Result<T>
{
    private readonly List<string> _error = new List<string>();
    
    public BadRequestResult(string error)
    {
        _error.Add(error);
    }
    public BadRequestResult(List<string> errors)
    {
        errors.ForEach(error => _error.Add(error));
    }

    public override ResultTypesEnum ResultType => ResultTypesEnum.BadRequest;

    public override ErrorResponse Errors => new ErrorResponse()
    {
        Status = 400,
        Message = "Bad Request",
        Errors = _error
    };

    public override T Data => default!;
}