using System.Net;
using System.Text.Json;
using FluentResults;
using MailerManager.Core.Common.Constants;
using MailerManager.Core.Services.MailRuManager;
using MailerManager.Core.Services.Postmaster;
using MailerManager.Infrastructure.Extensions.RestSharp;
using MailerManager.Infrastructure.Services.Postmaster.Constants;
using MailerManager.Infrastructure.Services.Postmaster.Exceptions;
using MailerManager.Infrastructure.Services.Postmaster.Responses;
using RestSharp;

namespace MailerManager.Infrastructure.Services.Postmaster;

public class PostmasterService(
    IPostmasterContext postmasterContext,
    IHttpClientFactory httpClientFactory,
    IMailRuAccessTokenManagerService accessTokenManagerService)
    : IPostmasterService
{
    private readonly RestClient _client = new(httpClientFactory.CreateClient(HttpClientNames.Postmaster));
    private readonly IPostmasterContext _context = postmasterContext;
    private readonly IMailRuAccessTokenManagerService _accessTokenManagerService = accessTokenManagerService;
    
    public async Task<Result> RunAsync()
    {
        _context.Token = await _accessTokenManagerService.GetAccessTokenAsync();

        Console.WriteLine($"{nameof(GetDomainsAsync).ToUpper()} RESULT:");
        var domains = await GetDomainsAsync();
        Console.WriteLine(JsonSerializer.Serialize(domains, new JsonSerializerOptions { WriteIndented = true }));
        
        Console.WriteLine(new string('-', Console.WindowWidth));
        
        var statisticsForSingle = await GetStatisticsForSingleDomainAsync(domains.First());
        Console.WriteLine($"{nameof(GetStatisticsForSingleDomainAsync).ToUpper()} RESULT:");
        Console.WriteLine(JsonSerializer.Serialize(statisticsForSingle, new JsonSerializerOptions { WriteIndented = true }));

        Console.WriteLine(new string('-', Console.WindowWidth));
        
        var statisticsForAll = await GetStatisticsForAllDomainsAsync();
        Console.WriteLine($"{nameof(GetStatisticsForAllDomainsAsync).ToUpper()} RESULT:");
        Console.WriteLine(JsonSerializer.Serialize(statisticsForAll, new JsonSerializerOptions { WriteIndented = true }));
        
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

    public async Task<DomainStatisticsModel> GetStatisticsForSingleDomainAsync(DomainModel domain)
    {
        return (await GetDomainStatisticsAsync(new()
        {
            Domain = domain.Domain,
            MsgType = QueryParameterValues.HotNews,
            DateFrom = DateTime.Now.AddMonths(-2)
        }))
        .First();
    }

    public async Task<List<DomainStatisticsModel>> GetStatisticsForAllDomainsAsync()
    {
        return await GetDomainStatisticsAsync(new()
        {
            DateFrom = DateTime.Now.AddMonths(-2)
        });
    }

    private async Task<List<DomainStatisticsModel>> GetDomainStatisticsAsync(QueryParameterOptions queryParameterOptions)
    {
        var request = new RestRequest("/ext-api/stat-list/", Method.Get);
        
        request
            .AddHeader(_context.Options.TokenType, _context.Token.AccessToken);
        
        request
            .AddQueryParameterWhereValueNotNull(QueryParameterNames.Domain, queryParameterOptions.Domain)
            .AddQueryParameterWhereValueNotNull(QueryParameterNames.DateFrom, queryParameterOptions.DateFrom?.ToString("yyyy-MM-dd"))
            .AddQueryParameterWhereValueNotNull(QueryParameterNames.DateTo, queryParameterOptions.DateTo?.ToString("yyyy-MM-dd"))
            .AddQueryParameterWhereValueNotNull(QueryParameterNames.MsgType, queryParameterOptions.MsgType);

        var response = await _client.ExecuteAsync<GetDomainDetailsResponse>(request);
        
        return response switch
        {
            { StatusCode: HttpStatusCode.Forbidden } => throw new AccessTokenRefreshRequiredException(),
            { Data: null } => throw new PostmasterResponseException("Data is null. Content: " + response.Content),
            { Data.Data: null } => throw new PostmasterResponseException("Mail data is null. Content: " + response.Content),
            { Data.Data.Count: <= 0 } => throw new PostmasterResponseException("Mail data is empty. Content: " + response.Content),

            var r => r.Data.Data
        };
    }
}