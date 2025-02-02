using System.Text.Json;
using MailerManager.Core.Clients.MailRu;
using MailerManager.Core.Services.MailRuManager;
using MailerManager.Infrastructure.Services.Postmaster;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MailerManager.Infrastructure.Services.MailRuManager;

public class MailRuAccessTokenManagerService(
    ILogger<MailRuAccessTokenManagerService> logger, 
    IOptions<PostmasterOptions> options, 
    IMailRuClient mailRuClient) 
    : IMailRuAccessTokenManagerService
{
    private const string FileName = "access_token.json";

    public async Task<IMailRuAccessToken> GetAccessTokenAsync()
    {
        IMailRuAccessToken? token = null;

        if (File.Exists(FileName))
        {
            try
            {
                token = JsonSerializer.Deserialize<MailRuAccessToken>(await File.ReadAllTextAsync(FileName));
            }
            catch (JsonException ex)
            {
                logger.LogWarning(ex.Message, ex);
            }
        }

        return token is not null && !string.IsNullOrEmpty(token.AccessToken) ? token : await RefreshAccessTokenWithSyncFileAsync();
    }

    public async Task<IMailRuAccessToken> RefreshAccessTokenAsync()
    {
        return await RefreshAccessTokenWithSyncFileAsync();
    }

    private async Task<IMailRuAccessToken> RefreshAccessTokenWithSyncFileAsync()
    {
        var result = await mailRuClient.RefreshAccessTokenAsync(options.Value.RefreshToken);

        if (!result.IsSuccess) throw new InvalidOperationException(result.Errors.First().Message);

        var token = new MailRuAccessToken(result.Value);
        
        await File.WriteAllTextAsync(FileName, JsonSerializer.Serialize(token));
        
        return token;
    }
}