using Microsoft.AspNetCore.OData.Query;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WebApplication
{
    public class ODataQueryOptionsFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var oDataQueryOptionsType = typeof(ODataQueryOptions);
            var oDataQueryOptionsParameter = context.ApiDescription.ParameterDescriptions.FirstOrDefault(
                p => oDataQueryOptionsType.IsAssignableFrom(p.Type));
            if (oDataQueryOptionsParameter != null)
            {
                operation.Parameters.Remove(operation.Parameters.First(p => p.Name == oDataQueryOptionsParameter.Name));
                operation.Parameters.Add(new OpenApiParameter()
                {
                    Name = "$expand",
                    In = ParameterLocation.Query
                });
                if (!operation.Parameters.Any(p => p.Name == "id"))
                {
                    operation.Parameters.Add(new OpenApiParameter()
                    {
                        Name = "$filter",
                        In = ParameterLocation.Query
                    });
                    operation.Parameters.Add(new OpenApiParameter()
                    {
                        Name = "$orderby",
                        In = ParameterLocation.Query
                    });
                    var top = new OpenApiParameter()
                    {
                        Name = "$top",
                        In = ParameterLocation.Query
                    };
                    top.Examples.Add("100", new OpenApiExample() { Value = new OpenApiInteger(100) });
                    operation.Parameters.Add(top);
                }
            }
        }
    }
}
