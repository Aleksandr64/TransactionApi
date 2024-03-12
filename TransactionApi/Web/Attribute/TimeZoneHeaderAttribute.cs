using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TransactionApi.Web.Attribute;

public class TimeZoneHeaderAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ActionArguments.ContainsKey("timeZoneOffsetInMinutes") || context.ActionArguments["timeZoneOffsetInMinutes"] == null)
        {
            var requestHeaders = context.HttpContext.Request.Headers;
            if (requestHeaders.ContainsKey("Time-Zone"))
            {
                var clientTimeZoneOffset = requestHeaders["Time-Zone"].ToString();

                if (int.TryParse(clientTimeZoneOffset, out int timeZoneOffsetInMinutes))
                {
                    context.ActionArguments["timeZoneOffsetInMinutes"] = timeZoneOffsetInMinutes;
                }
            }
        }
        base.OnActionExecuting(context);
    }
}