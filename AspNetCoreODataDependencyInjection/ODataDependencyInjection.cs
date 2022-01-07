using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCoreODataDependencyInjection;

/// <summary>
/// <see cref="TypeFilterAttribute"/> to enable AspNetCoreOData dependency injection
/// </summary>
public class ODataDependencyInjection : TypeFilterAttribute
{
    /// <summary>
    /// Initializes a new instance of <see cref="ODataDependencyInjection"/> class
    /// </summary>
    /// <param name="setupAction">the setup config</param>
    public ODataDependencyInjection(Action<IServiceCollection> setupAction) : base(typeof(ODataDependencyInjectionResourceFilter))
    {
        Arguments = new[] { setupAction };
    }
}
