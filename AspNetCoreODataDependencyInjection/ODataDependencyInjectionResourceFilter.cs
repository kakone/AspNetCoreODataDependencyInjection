using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Abstracts;
using Microsoft.AspNetCore.OData.Extensions;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OData;
using System.Reflection;

namespace AspNetCoreODataDependencyInjection;

/// <summary>
/// Resource filter to enable AspNetCoreOData dependency injection
/// </summary>
public class ODataDependencyInjectionResourceFilter : IResourceFilter
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ODataDependencyInjectionResourceFilter"/> class
    /// </summary>
    /// <param name="oDataOptionsAccessor"><see cref="ODataOptions"/> accessor</param>
    /// <param name="setupAction">the setup config</param>
    public ODataDependencyInjectionResourceFilter(IOptions<ODataOptions> oDataOptionsAccessor, Action<IServiceCollection> setupAction)
    {
        Services = new Lazy<IServiceProvider>(() =>
        {
            var oDataFeatureInterfaceType = typeof(IODataFeature);
            var defaultContainerBuilderType =
                oDataFeatureInterfaceType.Assembly.GetType($"{oDataFeatureInterfaceType.Namespace}.DefaultContainerBuilder")!;
            // var builder = new DefaultContainerBuilder();
            var builder = (IContainerBuilder)Activator.CreateInstance(defaultContainerBuilderType)!;
            builder.AddDefaultODataServices();
            builder.AddService(Microsoft.OData.ServiceLifetime.Singleton, sp => oDataOptionsAccessor.Value.QuerySettings);
            // builder.AddDefaultWebApiServices();
            (oDataFeatureInterfaceType.Assembly.GetType($"{oDataFeatureInterfaceType.Namespace}.ContainerBuilderExtensions")!)
                .GetMethod("AddDefaultWebApiServices")!.Invoke(null, new[] { builder });
            // setupAction.Invoke(builder.Services);
            setupAction?.Invoke((IServiceCollection)defaultContainerBuilderType
                .GetProperty("Services", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(builder)!);
            return builder.BuildContainer();
        });
    }

    private Lazy<IServiceProvider> Services { get; }

    /// <inheritdoc/>
    public void OnResourceExecuting(ResourceExecutingContext context)
    {
        var odataQueryOptionsType = typeof(ODataQueryOptions<>);
        if (!context.ActionDescriptor.Parameters.Any(p => p.ParameterType.IsGenericType && p.ParameterType.GetGenericTypeDefinition() == odataQueryOptionsType) &&
            !context.Filters.Any(f => f is EnableQueryAttribute))
        {
            return;
        }

        var odataFeature = context.HttpContext.ODataFeature();
        if (odataFeature.Services == null)
        {
            odataFeature.Services = Services.Value;
        }
    }

    /// <inheritdoc/>
    public void OnResourceExecuted(ResourceExecutedContext context)
    {
    }
}
