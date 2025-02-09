using FluentResults;
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
    
    public async Task<Result> RunAsync()
    {
        await AuthMailWizzAsync();
        return Result.Ok();
    }
    
    private async Task AuthMailWizzAsync()
    {
        using var playwright = await Playwright.CreateAsync();
        
        await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            ExecutablePath = _playwrightOptions.Value.ExecutablePath,
            Headless = _playwrightOptions.Value.Headless
        });
        
        var authResult = await _mailWizzAuthentication.AuthIfNeed(browser);
        
        _logger.LogInformation(authResult.IsFailed.ToString());

        await browser.CloseAsync();
    }
}