# AspNetCoreODataDependencyInjection
To enable AspNetCoreOData dependency injection for non-edm model

## Usage
```csharp
services.AddControllers(opt =>
{
    opt.Filters.Add(new ODataDependencyInjection(services =>
        services.AddSingleton<ODataUriResolver>(sp => new StringAsEnumResolver() { EnableCaseInsensitive = true })));
});
```
