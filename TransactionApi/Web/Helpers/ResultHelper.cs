using Microsoft.AspNetCore.Mvc;
using TransactionApi.Domain.ResultModels;

namespace TransactionApi.Web.Helpers;

//Static class for help create response
public static class ResultHelper
{
    public static ActionResult GetResponse<T>(this ControllerBase controller, Result<T> result)
    {
        return result.ResultType switch
        {
            ResultTypesEnum.Success => result.Data == null ? controller.NoContent() : controller.Ok(result.Data),
            ResultTypesEnum.BadRequest => controller.BadRequest(result.Errors),
            ResultTypesEnum.NotFound => controller.NotFound(result.Errors),
            _ => controller.BadRequest()
        };
    }
}