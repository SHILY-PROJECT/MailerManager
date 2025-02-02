using FluentResults;
using MailerManager.Core.Common.Constants;
using MailerManager.Core.Services.MailRuManager;
using MailerManager.Core.Services.Postmaster;
using Microsoft.Extensions.Options;
using RestSharp;

namespace MailerManager.Infrastructure.Services.Postmaster;

public class PostmasterService(
    IOptions<PostmasterOptions> options,
    IHttpClientFactory httpClientFactory,
    IMailRuAccessTokenManagerService accessTokenManagerService)
    : IPostmasterService
{
    private readonly RestClient _restClient = new(httpClientFactory.CreateClient(HttpClientNames.Postmaster));
    private readonly IOptions<PostmasterOptions> _options = options;
    private readonly IMailRuAccessTokenManagerService _accessTokenManagerService = accessTokenManagerService;

    public async Task<Result> RunAsync()
    {
        var token = await _accessTokenManagerService.GetAccessTokenAsync();
        
        await Task.Delay(TimeSpan.FromSeconds(1));
        
        return Result.Ok();
    }
}