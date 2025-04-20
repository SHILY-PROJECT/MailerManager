using FluentResults;
using MailerManager.Core.Actions;
using MailerManager.Core.Context;
using MailerManager.Core.Services.MailWizz;
using MailerManager.Infrastructure.Services.MailWizz;
using Microsoft.Extensions.Options;
using Microsoft.Playwright;

namespace MailerManager.Infrastructure.Actions.MailWizz;

[Action(ActionName)]
public class AuthenticationAction(IOptions<MailWizzOptions> options) : IAction
{
    private const string ActionName = "mail_wizz_authentication";
    
    private MailWizzOptions MailWizzOptions { get; } = options.Value;

    public async Task<Result> ExecuteAsync(MailerManagerContext context) => await AuthIfNeedAsync(context);
    
    private async Task<Result> AuthIfNeedAsync(MailerManagerContext context)
    {
        try
        {
            var browser = await context.GetBrowserAsync();
            await AuthenticateAsync(browser);
            return Result.Ok().WithSuccess($"{ActionName} Аутентификация успешно пройдена");
        }
        catch (Exception ex)
        {
            return Result.Fail(new Error($"{ActionName} Ошибка: {ex.Message}").WithMetadata(nameof(Exception), ex));
        }
    }

    private async Task AuthenticateAsync(IBrowser browser)
    {
        var page = await browser.NewPageAsync();
        await page.GotoAsync(MailWizzOptions.AuthPageUrl);

        await page.WaitForSelectorAsync(XPathMailWizzPage.Login);

        await page.FillAsync(XPathMailWizzPage.Login, MailWizzOptions.Login);
        await page.FillAsync(XPathMailWizzPage.Password, MailWizzOptions.Password);
        await page.ClickAsync(XPathMailWizzPage.Submit);

        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }
}