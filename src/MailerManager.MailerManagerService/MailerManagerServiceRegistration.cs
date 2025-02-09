using MailerManager.Core.Services.MailerManager;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MailerManager.MailerManagerService;

public static class MailerManagerServiceRegistration
{
    public static IServiceCollection AddMailerManagerServices(this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddOptions(configuration);
    }
    
    private static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
    {
        return services;
    }
}