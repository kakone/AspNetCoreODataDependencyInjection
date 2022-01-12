using AspNetCoreODataDependencyInjection;
using Microsoft.OData.UriParser;

namespace WebApplication
{
    public class StringAsEnumResolverFilterAttribute : ODataDependencyInjection
    {
        public StringAsEnumResolverFilterAttribute() :
            base(services => services.AddSingleton<ODataUriResolver>(sp => new StringAsEnumResolver() { EnableCaseInsensitive = true }))
        {
        }
    }
}
