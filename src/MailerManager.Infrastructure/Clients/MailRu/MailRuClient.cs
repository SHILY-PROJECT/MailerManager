using MailerManager.Core.Clients.MailRu;
using MailerManager.Core.Common.Constants;
using Microsoft.Extensions.Options;
using RestSharp;

namespace MailerManager.Infrastructure.Clients.MailRu;

public class MailRuClient(
    IOptions<MailRuOptions> options,
    IHttpClientFactory httpClientFactory) 
    : RestClient(httpClientFactory.CreateClient(HttpClientNames.MailRu)), IMailRuClient
{
    private readonly IOptions<MailRuOptions> _options = options;

    public async Task<string> RefreshAccessTokenAsync(string refreshToken)
    {
        throw new NotImplementedException();
    }
}