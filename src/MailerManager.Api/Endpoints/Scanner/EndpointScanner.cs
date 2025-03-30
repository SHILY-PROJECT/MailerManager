using System.Reflection;

namespace MailerManager.Api.Endpoints.Scanner;

public static class EndpointScanner
{
    public static IServiceCollection ScanEndpoints(this IServiceCollection services, Assembly assembly)
    {
        var endpointType = typeof(IEndpoint);

        Array.ForEach(
            assembly.GetTypes().Where(t => endpointType.IsAssignableFrom(t) && t is { IsClass: true, IsAbstract: false }).ToArray(), 
            type => services.AddTransient(endpointType, type));
        
        return services;
    }
}