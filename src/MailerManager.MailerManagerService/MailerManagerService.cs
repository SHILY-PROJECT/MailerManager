using FluentResults;
using MailerManager.Core.Services.MailerManager;
using MailerManager.Core.Services.Postmaster;

namespace MailerManager.MailerManagerService;

public class MailerManagerService(IPostmasterService postmasterService) : IMailerManagerService
{
    private readonly IPostmasterService _postmasterService = postmasterService;

    public async Task<Result> RunAsync()
    {
        var result = await _postmasterService.RunAsync();
        
        return Result.Ok();
    }
}