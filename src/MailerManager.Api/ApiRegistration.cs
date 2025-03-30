using System.Reflection;
using MailerManager.Api.Endpoints.Scanner;
using MailerManager.Core;
using MailerManager.Core.Tools.DependencyInjection;
using MailerManager.Infrastructure;
using MailerManager.MailerManagerService;

namespace MailerManager.Api;

public static class ApiRegistration
{
    public static IServiceCollection AddApi(this IServiceCollection services, Assembly assembly)
    {
        services.ScanEndpoints(assembly);
        services.ScanDependencies([assembly, typeof(CoreRegistration).Assembly, typeof(InfrastructureRegistration).Assembly, typeof(MailerManagerServiceRegistration).Assembly]);
        
        return services;
    }
    
    public static void AddEndpoints(this WebApplication app) => 
        Array.ForEach(app.Services.GetServices<IEndpoint>().ToArray(), endpoint => endpoint.AddEndpoints(app));
}