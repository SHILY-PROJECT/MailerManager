using FluentResults;
using MailerManager.Core.Context;
using MailerManager.Core.Services.MailerManager;
using MailerManager.Core.Services.MailWizz;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Playwright;

namespace MailerManager.Infrastructure.Services.MailWizz;

public class MailWizzService(
    ILogger<MailWizzService> logger, 
    IMailWizzAuthentication mailWizzAuthentication, 
    IOptions<MailWizzOptions> mailWizzOptions, 
    IOptions<PlaywrightOptions> playwrightOptions) : IMailWizzService
{
    private readonly ILogger<MailWizzService> _logger = logger;
    private readonly IMailWizzAuthentication _mailWizzAuthentication = mailWizzAuthentication;
    private readonly IOptions<MailWizzOptions> _mailWizzOptions = mailWizzOptions;
    private readonly IOptions<PlaywrightOptions> _playwrightOptions = playwrightOptions;

    public MailerManagerContext Context { get; set; } = null!;
    
    public async Task<Result> ExecuteAsync(MailerManagerContext context)
    {
        Context = context;
        
        await AuthMailWizzAsync();
        return Result.Ok();
    }
    
    private async Task AuthMailWizzAsync()
    {
        var browser = await Context.GetBrowserAsync();
        
        var authResult = await _mailWizzAuthentication.AuthIfNeed(browser);
        
        _logger.LogInformation(authResult.IsFailed.ToString());

        await browser.CloseAsync();
    }
}