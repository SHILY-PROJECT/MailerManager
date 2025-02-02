using MailerManager.Core;
using MailerManager.Core.Common.DependencyInjection;
using MailerManager.Infrastructure;
using MailerManager.MailerManagerService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MailerManager.ConsoleApp;

public static class ConsoleAppRegistration
{
    private const string ConfigurationName = "appsettings.json";
    
    public static IServiceCollection AddConsoleApp(this IServiceCollection services)
    {
        var cfg = services.CreateConfiguration();
        
        services.AddDependencies();
        services.AddHttpClient();
        services.AddCore();
        services.AddInfrastructure(cfg);
        services.AddMailerManagerServices();

        return services;
    }

    private static  IConfigurationRoot CreateConfiguration(this IServiceCollection services)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(ConfigurationName, optional: false, reloadOnChange: true)
            .AddUserSecrets<Program>()
            .Build();

        services.AddSingleton<IConfiguration>(configuration);

        return configuration;
    }

    private static IServiceCollection AddDependencies(this IServiceCollection services)
    {
        var interfaceToLifetimeMap = new Dictionary<Type, ServiceLifetime>
        {
            [typeof(ITransientDependency)] = ServiceLifetime.Transient,
            [typeof(IScopedDependency)] = ServiceLifetime.Scoped,
            [typeof(ISingletonDependency)] = ServiceLifetime.Singleton,
        };
        
        services.Scan(scan => scan
            .FromAssemblies(
                typeof(ConsoleAppRegistration).Assembly, 
                typeof(CoreRegistration).Assembly,
                typeof(InfrastructureRegistration).Assembly,
                typeof(MailerManagerServiceRegistration).Assembly)
            .AddClasses(classes => classes.AssignableToAny(interfaceToLifetimeMap.Keys))
            .AsImplementedInterfaces()
            .WithLifetime(type =>
            {
                var implementedInterfaces = type.GetInterfaces();

                foreach (var interfaceType in implementedInterfaces)
                {
                    if (interfaceToLifetimeMap.TryGetValue(interfaceType, out var lifetime)) return lifetime;

                    var baseInterfaces = GetAllBaseInterfaces(interfaceType);
                    
                    if (baseInterfaces.Any(baseInterface => interfaceToLifetimeMap.TryGetValue(baseInterface, out lifetime))) return lifetime;
                }
                
                throw new InvalidOperationException($"Type '{type.Name}' does not match any known interface");
            }));
                
        return services;
    }
    
    private static IEnumerable<Type> GetAllBaseInterfaces(Type? type)
    {
        if (type == null || type == typeof(object)) yield break;

        foreach (var interfaceType in type.GetInterfaces())
        {
            yield return interfaceType;
            
            foreach (var baseInterface in GetAllBaseInterfaces(interfaceType)) yield return baseInterface;
        }
    }
}