using MailerManager.Core.Handlers.JobTemplate;
using MailerManager.Core.Services.MailerManager;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace MailerManager.Core.Context;

public class MailerManagerContextCreator
{
    private MailerManagerContextCreator(CreateJobTemplateCommand createJobTemplateCommand, IServiceProvider serviceProvider) =>
        Context = new MailerManagerContext(createJobTemplateCommand, serviceProvider.GetRequiredService<IOptions<PlaywrightOptions>>());
    
    private MailerManagerContext Context { get; }
    
    public static MailerManagerContext CreateContext(CreateJobTemplateCommand createJobTemplateCommand, IServiceProvider serviceProvider) =>
        new MailerManagerContextCreator(createJobTemplateCommand, serviceProvider).Context;
}