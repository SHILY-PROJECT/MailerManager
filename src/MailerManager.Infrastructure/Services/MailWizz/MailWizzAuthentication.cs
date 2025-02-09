using FluentResults;
using MailerManager.Core.Services.MailWizz;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Playwright;

namespace MailerManager.Infrastructure.Services.MailWizz;

public class MailWizzAuthentication(ILogger<MailWizzAuthentication> logger, IOptions<MailWizzOptions> options) : IMailWizzAuthentication
{
    private const string AuthPageUrl = "https://wizzardpro.online/customer/index.php/campaigns/index";
    
    private MailWizzOptions MailWizzOptions { get; } = options.Value;
    
    public async Task<Result> AuthIfNeed(IBrowser browser)
    {
        try
        {
            await Authenticate(browser);
            
            logger.LogInformation($"{GetType().Name} Authentication completed");
            
            return Result.Ok();
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return Result.Fail(ex.Message);
        }
    }

    private async Task Authenticate(IBrowser browser)
    {
        var page = await browser.NewPageAsync();
        await page.GotoAsync(AuthPageUrl);

        await page.WaitForSelectorAsync(XPathMailWizzPage.Login);

        await page.FillAsync(XPathMailWizzPage.Login, MailWizzOptions.Login);
        await page.FillAsync(XPathMailWizzPage.Password, MailWizzOptions.Password);
        await page.ClickAsync(XPathMailWizzPage.Submit);

        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }
}