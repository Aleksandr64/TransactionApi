using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using TransactionApi.Web.Attribute;

namespace TransactionApi.Web.SwaggerOptions;

/// <inheritdoc />
public class AddTimeZoneHeaderFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var hasTimeZoneHeaderAttribute = context.ApiDescription.ActionDescriptor.EndpointMetadata
            .Any(em => em is TimeZoneHeaderAttribute);

        if (hasTimeZoneHeaderAttribute)
        {
            if (operation.Parameters == null)
            {
                operation.Parameters = new List<OpenApiParameter>();
            }
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "Time-Zone",
                In = ParameterLocation.Header,
                Description = "Client's Time Zone",
                Required = false
            });
        }
    }
}
