using System.Reflection;
using MailerManager.Core;
using MailerManager.Core.Tools.DependencyInjection;
using MailerManager.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MailerManager.ConsoleApp;

public static class ConsoleAppRegistration
{
    private const string ConfigurationName = "appsettings.json";
    
    public static IServiceCollection AddConsoleApp(this IServiceCollection services, Assembly assembly)
    {
        var cfg = services.CreateConfiguration();
        
        services.AddHttpClient();
        services.AddCore();
        services.AddInfrastructure(cfg);
        services.ScanDependencies([assembly,  typeof(CoreRegistration).Assembly, typeof(InfrastructureRegistration).Assembly]);

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
}