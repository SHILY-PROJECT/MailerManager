using FluentResults;
using MailerManager.Core.Context;
using MailerManager.Core.Services.MailerManager;
using MailerManager.Core.Services.MailWizz;
using MailerManager.Core.Services.Postmaster;

namespace MailerManager.Infrastructure.Services.MailerManager;

public class MailerManagerService(
    IServiceProvider serviceProvider, 
    IPostmasterService postmasterService, 
    IMailWizzService mailWizzService) : IMailerManagerService
{
    public async Task<Result> RunAsync()
    {
        var context = MailerManagerContextCreator.CreateContext(new(), serviceProvider);
        // var result = await _postmasterService.RunAsync();
        var mailWizzResult = await mailWizzService.ExecuteAsync(context);

        
        
        return Result.Ok();
    }

    
}