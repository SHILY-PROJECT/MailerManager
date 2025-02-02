using FluentResults;
using MailerManager.Core.Clients.MailRu;
using MailerManager.Core.Common.Constants;
using MailerManager.Core.Services.Postmaster;
using Microsoft.Extensions.Options;
using RestSharp;

namespace MailerManager.Infrastructure.Services.Postmaster;

public class PostmasterService(
    IOptions<PostmasterOptions> options,
    IHttpClientFactory httpClientFactory,
    IMailRuClient mailRuClient)
    : IPostmasterService
{
    private readonly RestClient _restClient = new(httpClientFactory.CreateClient(HttpClientNames.Postmaster));
    private readonly IOptions<PostmasterOptions> _options = options;

    public async Task<Result> RunAsync()
    {
        await Task.Delay(TimeSpan.FromSeconds(1));
        
        return Result.Ok();
    }
}