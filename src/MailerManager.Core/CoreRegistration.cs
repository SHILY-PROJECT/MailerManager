using MailerManager.Core.Behaviors.Global;
using MailerManager.Core.Handlers.Global;
using MediatR.Pipeline;
using Microsoft.Extensions.DependencyInjection;

namespace MailerManager.Core;

public static class CoreRegistration
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        return services.AddMediatRComponents();
    }
    
    private static IServiceCollection AddMediatRComponents(this IServiceCollection services)
    {
        services
            .AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(CoreRegistration).Assembly);
                cfg.AddOpenBehavior(typeof(GlobalLoggingBehavior<,>));
            })
            .AddTransient(typeof(IRequestExceptionHandler<,,>), typeof(GlobalRequestExceptionHandler<,,>));

        return services;
    }
}