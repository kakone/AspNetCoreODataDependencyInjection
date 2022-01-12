using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace WebApplication
{
    public static class SwaggerGenOptionsExtensions
    {
        public static SwaggerGenOptions MapODataQueryOptionsTypes(this SwaggerGenOptions options)
        {
            var oDataQueryOptionsType = typeof(ODataQueryOptions);
            var controllerBaseType = typeof(ControllerBase);
            var oDataQueryOptionsTypes = new List<Type>();
            foreach (var controllerType in Assembly.GetExecutingAssembly().GetTypes().Where(t => controllerBaseType.IsAssignableFrom(t)))
            {
                foreach (var method in controllerType.GetMethods().Where(m => m.GetCustomAttribute<HttpGetAttribute>() != null))
                {
                    foreach (var parameter in method.GetParameters().Where(
                        p => oDataQueryOptionsType.IsAssignableFrom(p.ParameterType) && !oDataQueryOptionsTypes.Contains(p.ParameterType)))
                    {
                        options.MapType(parameter.ParameterType, () => new OpenApiSchema { Type = "string" });
                        oDataQueryOptionsTypes.Add(parameter.ParameterType);
                    }
                }
            }
            return options;
        }
    }
}
