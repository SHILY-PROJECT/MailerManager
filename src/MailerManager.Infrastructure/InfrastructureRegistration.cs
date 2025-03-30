using MailerManager.Core.Constants;
using MailerManager.Core.Services.MailerManager;
using MailerManager.Core.Services.MailWizz;
using MailerManager.Core.Services.Postmaster;
using MailerManager.Infrastructure.Clients.MailRu;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace MailerManager.Infrastructure;

public static class InfrastructureRegistration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient();
        services.AddOptions(configuration);
        services.AddHttpClients();
        
        return services;
    }
    
    private static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<MailRuOptions>()
            .Bind(configuration.GetSection(nameof(MailRuOptions)))
            .ValidateDataAnnotations();
        
        services.AddOptions<PostmasterOptions>()
            .Bind(configuration.GetSection(nameof(PostmasterOptions)))
            .ValidateDataAnnotations();
        
        services.AddOptions<PlaywrightOptions>()
            .Bind(configuration.GetSection(nameof(PlaywrightOptions)))
            .ValidateDataAnnotations();
        
        services.AddOptions<MailWizzOptions>()
            .Bind(configuration.GetSection(nameof(MailWizzOptions)))
            .ValidateDataAnnotations();
        
        return services;
    }
    
    private static IServiceCollection AddHttpClients(this IServiceCollection services)
    {
        services.AddHttpClient(HttpClientNames.MailRu, (service, client) => client.BaseAddress = new(service.GetRequiredService<IOptions<PostmasterOptions>>().Value.AuthUrl));
        
        services
            .AddHttpClient(HttpClientNames.Postmaster, (service, client) => client.BaseAddress = new(service.GetRequiredService<IOptions<PostmasterOptions>>().Value.Url))
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler { ServerCertificateCustomValidationCallback = (_, _, _, _) => true });
        
        return services;
    }
}