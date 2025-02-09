using FluentResults;
using MailerManager.Core.Services.MailerManager;
using MailerManager.Core.Services.MailWizz;
using MailerManager.Core.Services.Postmaster;
using Microsoft.Extensions.Options;
using Microsoft.Playwright;

namespace MailerManager.MailerManagerService;

public class MailerManagerService(IPostmasterService postmasterService, IMailWizzService mailWizzService) : IMailerManagerService
{
    private readonly IPostmasterService _postmasterService = postmasterService;
    private readonly IMailWizzService _mailWizzService = mailWizzService;

    public async Task<Result> RunAsync()
    {
        // var result = await _postmasterService.RunAsync();
        var mailWizzResult = await _mailWizzService.RunAsync();

        
        
        return Result.Ok();
    }

    
}