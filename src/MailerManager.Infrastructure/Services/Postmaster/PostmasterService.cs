using System.Net;
using FluentResults;
using MailerManager.Core.Common.Constants;
using MailerManager.Core.Services.MailRuManager;
using MailerManager.Core.Services.Postmaster;
using MailerManager.Infrastructure.Services.Postmaster.Exceptions;
using MailerManager.Infrastructure.Services.Postmaster.Responses;
using Microsoft.Extensions.Options;
using RestSharp;
using RestSharp.Authenticators;

namespace MailerManager.Infrastructure.Services.Postmaster;

public class PostmasterService(
    PostmasterContext postmasterContext,
    IOptions<PostmasterOptions> options,
    IHttpClientFactory httpClientFactory,
    IMailRuAccessTokenManagerService accessTokenManagerService)
    : IPostmasterService
{
    private readonly RestClient _client = new(httpClientFactory.CreateClient(HttpClientNames.Postmaster));
    private readonly PostmasterContext _context = postmasterContext;
    private readonly IOptions<PostmasterOptions> _options = options;
    private readonly IMailRuAccessTokenManagerService _accessTokenManagerService = accessTokenManagerService;
    
    public async Task<Result> RunAsync()
    {
        _context.Token = await _accessTokenManagerService.GetAccessTokenAsync();

        var domains = await GetDomainsAsync();
        var statistic = await GetStatisticByDomainAsync(domains.First());
        
        await Task.Delay(TimeSpan.FromSeconds(1));
        
        return Result.Ok();
    }

    public async Task<List<DomainModel>> GetDomainsAsync()
    {
        var request = new RestRequest("/ext-api/reg-list/", Method.Get);
        
        request.AddHeader(_context.Options.TokenType, _context.Token.AccessToken);
        
        var response = await _client.ExecuteAsync<GetDomainsResponse>(request) switch
        {
            { StatusCode: HttpStatusCode.Forbidden } => throw new AccessTokenRefreshRequiredException(),
            { Data: null } => throw new PostmasterResponseException("Data is null"),
            { Data.Domains: null } => throw new PostmasterResponseException("Domains is null"),
            { Data.Domains.Count: <= 0 } => throw new PostmasterResponseException("Domains is empty"),
            
            var r => r.Data.Domains 
        };
        
        return response;
    }

    public async Task<DetailStatisticModel> GetStatisticByDomainAsync(DomainModel domain)
    {
        var request = new RestRequest("/ext-api/stat-list/", Method.Get);
        
        request.AddHeader(_context.Options.TokenType, _context.Token.AccessToken);
        
        request.AddQueryParameter("domain", domain.Domain);
        request.AddQueryParameter("date_from", "2025-01-01");
        request.AddQueryParameter("date_to", "2025-02-03");
        request.AddQueryParameter("msgtype", "hotnews");

        var response = await _client.ExecuteAsync<GetDomainDetailsResponse>(request) switch
        {
            { StatusCode: HttpStatusCode.Forbidden } => throw new AccessTokenRefreshRequiredException(),
            { Data: null } => throw new PostmasterResponseException("Data is null"),
            { Data.Data: null } => throw new PostmasterResponseException("Mail data is null"),
            { Data.Data.Count: <= 0 } => throw new PostmasterResponseException("Mail data is empty"),

            var r => r.Data.Data.First()
        };
        
        return response;
    }
}