using FluentResults;
using MailerManager.Core.Clients.MailRu;
using MailerManager.Core.Constants;
using Microsoft.Extensions.Logging;
using RestSharp;

namespace MailerManager.Infrastructure.Clients.MailRu;

public class MailRuClient(ILogger<MailRuClient> logger, IHttpClientFactory httpClientFactory) : RestClient(httpClientFactory.CreateClient(HttpClientNames.MailRu)), IMailRuClient
{
    public async Task<Result<string>> RefreshAccessTokenAsync(string refreshToken)
    {
        string msg;

        try
        {
            if (string.IsNullOrWhiteSpace(refreshToken)) throw new ArgumentNullException(nameof(refreshToken));
            
            var request = new RestRequest("/token", Method.Post);
        
            request.AddParameter("client_id", "postmaster_api_client");
            request.AddParameter("grant_type", "refresh_token");
            request.AddParameter("refresh_token", refreshToken);
            
            var response = await this.ExecuteAsync<RefreshAccessTokenResponse>(request);

            if (response.IsSuccessful)
            {
                logger.LogInformation(msg = "Access token refreshed successfully");
                
                return Result.Ok(response.Data!.AccessToken!).WithSuccess(msg);
            }

            logger.LogWarning($"{msg = "No access token refreshed"}. {nameof(response.Content)}: {response.Content}");
            
            return Result.Fail(msg);
        }
        catch (Exception ex)
        {
            logger.LogWarning(msg = ex.Message);
        }
        
        return Result.Fail(msg);
    }
}