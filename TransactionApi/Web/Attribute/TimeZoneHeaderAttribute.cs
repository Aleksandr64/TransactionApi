using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;

namespace TransactionApi.Web.Attribute;

public class TimeZoneHeaderAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ActionArguments.ContainsKey("timeZoneOffsetInMinutes") || context.ActionArguments["timeZoneOffsetInMinutes"] == null)
        {
            var requestHeaders = context.HttpContext.Request.Headers;
            if (requestHeaders.TryGetValue("Time-Zone", out var header))
            {
                var clientTimeZone = header.ToString();

                if (!clientTimeZone.IsNullOrEmpty())
                {
                    context.ActionArguments["timeZone"] = clientTimeZone;
                }
            }
        }
        base.OnActionExecuting(context);
    }
}