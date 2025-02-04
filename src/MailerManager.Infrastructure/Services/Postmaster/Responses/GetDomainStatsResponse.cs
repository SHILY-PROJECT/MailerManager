using System.Text.Json.Serialization;

namespace MailerManager.Infrastructure.Services.Postmaster.Responses;

public class GetDomainStatsResponse
{
    [JsonPropertyName("ok")]
    public bool Ok { get; set; }
    
    [JsonPropertyName("data")]
    public List<DomainStatsModel> Data { get; set; }
}

public class DomainStatsModel
{
    [JsonPropertyName("domain")]
    public string Domain { get; set; }

    [JsonPropertyName("delivered")]
    public int Delivered { get; set; }

    [JsonPropertyName("messages_sent")]
    public int MessagesSent { get; set; }

    [JsonPropertyName("deleted_read")]
    public int DeletedRead { get; set; }

    [JsonPropertyName("spam")]
    public int Spam { get; set; }

    [JsonPropertyName("read")]
    public int Read { get; set; }

    [JsonPropertyName("spam_percent")]
    public double SpamPercent { get; set; }

    [JsonPropertyName("probably_spam_percent")]
    public double ProbablySpamPercent { get; set; }

    [JsonPropertyName("complaints")]
    public int Complaints { get; set; }

    [JsonPropertyName("deleted_unread")]
    public int DeletedUnread { get; set; }

    [JsonPropertyName("probably_spam")]
    public int ProbablySpam { get; set; }
}