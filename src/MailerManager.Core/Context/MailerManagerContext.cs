using MailerManager.Core.Handlers.JobTemplate;
using MailerManager.Core.Services.MailerManager;
using Microsoft.Extensions.Options;
using Microsoft.Playwright;

namespace MailerManager.Core.Context;

public class MailerManagerContext(CreateJobTemplateCommand command,IOptions<PlaywrightOptions> playwrightOptions)
{
    public CreateJobTemplateCommand JobTemplate { get; set; }
    private IBrowser? Browser { get; set; }
    
    

    public async Task <IBrowser> GetBrowserAsync()
    {
        if (Browser is not null) return Browser;
        
        using var playwright = await Playwright.CreateAsync();

        await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            ExecutablePath = playwrightOptions.Value.ExecutablePath,
            Headless = playwrightOptions.Value.Headless
        });

        Browser = browser;

        return Browser;
    }
}