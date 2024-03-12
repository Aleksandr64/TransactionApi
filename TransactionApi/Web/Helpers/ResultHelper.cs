using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using Microsoft.AspNetCore.Mvc;
using TransactionApi.Domain.DTOs;
using TransactionApi.Domain.ResultModels;

namespace TransactionApi.Web.Helpers;

//Static class for help create response
public static class ResultHelper
{
    public static ActionResult GetResponse<T>(this ControllerBase controller, Result<T> result)
    {
        switch (result.ResultType)
        {
            case ResultTypesEnum.Success:
                return result.Data == null ? controller.NoContent() : controller.Ok(result.Data);
            case ResultTypesEnum.BadRequest:
                return controller.BadRequest(result.Errors);
            case ResultTypesEnum.NotFound:
                return controller.NotFound(result.Errors);
            case ResultTypesEnum.File:
                if (result.Data is FileResponse fileResponse)
                {
                    return controller.File(fileResponse.Content, fileResponse.ContentType, fileResponse.FileName);
                }
                return controller.StatusCode(500);
            default:
                return controller.BadRequest();
        }
    }
}