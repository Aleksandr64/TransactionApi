using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace TransactionApi.Web.SwaggerOptions;

public class AddTimeZoneHeaderFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
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