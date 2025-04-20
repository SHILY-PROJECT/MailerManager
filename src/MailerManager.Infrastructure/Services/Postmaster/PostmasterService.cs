using System.Net;
using System.Text.Json;
using FluentResults;
using MailerManager.Core.Constants;
using MailerManager.Core.Services.MailRu;
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
        
        var statisticsForSingle = await GetStatsForSingleDomainAsync(domains.First());
        Console.WriteLine($"{nameof(GetStatsForSingleDomainAsync).ToUpper()} RESULT:");
        Console.WriteLine(JsonSerializer.Serialize(statisticsForSingle, new JsonSerializerOptions { WriteIndented = true }));

        Console.WriteLine(new string('-', Console.WindowWidth));
        
        var statisticsForAll = await GetStatsForAllDomainsAsync();
        Console.WriteLine($"{nameof(GetStatsForAllDomainsAsync).ToUpper()} RESULT:");
        Console.WriteLine(JsonSerializer.Serialize(statisticsForAll, new JsonSerializerOptions { WriteIndented = true }));

        foreach (var domain in domains)
        {
            Console.WriteLine(new string('-', Console.WindowWidth));
            var domainStatsOfDays = await GetDomainStatsOfDaysAsync(domain);

            if (domainStatsOfDays.Data.Any())
            {
                Console.WriteLine($"{nameof(GetStatsForAllDomainsAsync).ToUpper()} RESULT:");
                Console.WriteLine(JsonSerializer.Serialize(domainStatsOfDays, new JsonSerializerOptions { WriteIndented = true }));
            }
        }

        await Task.Delay(TimeSpan.FromSeconds(1));
        
        return Result.Ok();
    }

    public async Task<List<DomainModel>> GetDomainsAsync()
    {
        var request = new RestRequest("/ext-api/reg-list/");
        
        request.AddHeader(_context.Options.TokenType, _context.Token.AccessToken);
        
        var response = await _client.ExecuteAsync<GetDomainsResponse>(request);
        
        return response switch
        {
            { StatusCode: HttpStatusCode.Forbidden } => throw new AccessTokenRefreshRequiredException(),
            { Data: null } => throw new PostmasterResponseException("Data is null. Content: " + response.Content),
            { Data.Domains: null } => throw new PostmasterResponseException("Domains is null. Content: " + response.Content),
            { Data.Domains.Count: <= 0 } => throw new PostmasterResponseException("Domains is empty. Content: " + response.Content),
            
            _ => response.Data.Domains 
        };
    }

    public async Task<DomainStatsModel> GetStatsForSingleDomainAsync(DomainModel domain)
    {
        return (await GetDomainStatsAsync(new QueryParameterOptions
        {
            Domain = domain.Domain,
            DateFrom = DateTime.Now.AddMonths(-5),
            // MsgType = QueryParameterValues.HotNews
        }))
        .First();
    }

    public async Task<List<DomainStatsModel>> GetStatsForAllDomainsAsync()
    {
        return await GetDomainStatsAsync(new QueryParameterOptions
        {
            DateFrom = DateTime.Now.AddMonths(-2)
        });
    }

    public async Task<DomainStatsOfDays> GetDomainStatsOfDaysAsync(DomainModel domain)
    {
        return await GetDomainStatsOfDaysAsync(new QueryParameterOptions
        {
            Domain = domain.Domain,
            DateFrom = DateTime.Now.AddMonths(-2),
            MsgType = QueryParameterValues.HotNews
        });
    }

    public async Task<DomainStatsOfDays> GetDomainStatsOfDaysAsync(QueryParameterOptions queryParameterOptions)
    {
        var request = new RestRequest("/ext-api/stat-list-detailed/");
        
        request
            .AddHeader(_context.Options.TokenType, _context.Token.AccessToken);
        
        request
            .AddQueryParameterWhereValueNotNull(QueryParameterNames.Domain, queryParameterOptions.Domain)
            .AddQueryParameterWhereValueNotNull(QueryParameterNames.DateFrom, queryParameterOptions.DateFrom?.ToString("yyyy-MM-dd"))
            .AddQueryParameterWhereValueNotNull(QueryParameterNames.DateTo, queryParameterOptions.DateTo?.ToString("yyyy-MM-dd"))
            .AddQueryParameterWhereValueNotNull(QueryParameterNames.MsgType, queryParameterOptions.MsgType);

        var response = await _client.ExecuteAsync<GetDomainStatsDaysResponse>(request);

        return response switch
        {
            { StatusCode: HttpStatusCode.Forbidden } => throw new AccessTokenRefreshRequiredException(),
            { Data: null } => throw new PostmasterResponseException("Data is null. Content: " + response.Content),
            { Data.Data: null } => throw new PostmasterResponseException("Mail data is null. Content: " + response.Content),
            { Data.Data.Count: <= 0 } => throw new PostmasterResponseException("Mail data is empty. Content: " + response.Content),

            _ => response.Data.Data.First()
        };
    }

    private async Task<List<DomainStatsModel>> GetDomainStatsAsync(QueryParameterOptions queryParameterOptions)
    {
        var request = new RestRequest("/ext-api/stat-list/");
        
        request
            .AddHeader(_context.Options.TokenType, _context.Token.AccessToken);
        
        request
            .AddQueryParameterWhereValueNotNull(QueryParameterNames.Domain, queryParameterOptions.Domain)
            .AddQueryParameterWhereValueNotNull(QueryParameterNames.DateFrom, queryParameterOptions.DateFrom?.ToString("yyyy-MM-dd"))
            .AddQueryParameterWhereValueNotNull(QueryParameterNames.DateTo, queryParameterOptions.DateTo?.ToString("yyyy-MM-dd"))
            .AddQueryParameterWhereValueNotNull(QueryParameterNames.MsgType, queryParameterOptions.MsgType);

        var response = await _client.ExecuteAsync<GetDomainStatsResponse>(request);
        
        return response switch
        {
            { StatusCode: HttpStatusCode.Forbidden } => throw new AccessTokenRefreshRequiredException(),
            { Data: null } => throw new PostmasterResponseException("Data is null. Content: " + response.Content),
            { Data.Data: null } => throw new PostmasterResponseException("Mail data is null. Content: " + response.Content),
            { Data.Data.Count: <= 0 } => throw new PostmasterResponseException("Mail data is empty. Content: " + response.Content),

            _ => response.Data.Data
        };
    }
}