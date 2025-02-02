using Microsoft.Extensions.DependencyInjection;

namespace MailerManager.MailerManagerService;

public static class MailerManagerServiceRegistration
{
    public static IServiceCollection AddMailerManagerServices(this IServiceCollection services)
    {
        return services;
    }
}