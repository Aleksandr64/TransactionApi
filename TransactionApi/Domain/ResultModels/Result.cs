﻿using TransactionApi.Domain.DTOs;

namespace TransactionApi.Domain.ResultModels;

//Base class for easier work with request
public abstract class Result<T>
{
    public abstract T Data { get; }
    public abstract ErrorResponse Errors { get; }
    public abstract ResultTypesEnum ResultType { get; }
}