using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MailerManager.Core.Services.Postmaster;

public class PostmasterOptions
{
    [Required]
    [JsonPropertyName("url")]
    public string Url { get; set; }
    
    [Required]
    public string AuthUrl { get; set; }
    
    [Required]
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }

    [Required]
    [JsonPropertyName("token_type")]
    public string TokenType { get; set; }

    [Required]
    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; set; }

    [Required]
    [JsonPropertyName("refresh_token")]
    public string RefreshToken { get; set; }
}