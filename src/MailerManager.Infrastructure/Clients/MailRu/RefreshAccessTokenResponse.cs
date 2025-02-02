using System.Text.Json.Serialization;

namespace MailerManager.Infrastructure.Clients.MailRu;

public class RefreshAccessTokenResponse
{
    [JsonPropertyName("expires_in")]
    public int? ExpiresIn { get; set; }

    [JsonPropertyName("access_token")]
    public string? AccessToken { get; set; }
}