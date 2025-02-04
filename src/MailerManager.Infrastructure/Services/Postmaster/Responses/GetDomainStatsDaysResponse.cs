using System.Text.Json.Serialization;

namespace MailerManager.Infrastructure.Services.Postmaster.Responses;

public class GetDomainStatsDaysResponse
{
    [JsonPropertyName("ok")]
    public bool Ok { get; set; }

    [JsonPropertyName("data")]
    public List<DomainStatsOfDays> Data { get; set; }
}

public class DomainStatsOfDays
{
    [JsonPropertyName("domain")]
    public string Domain { get; set; }

    [JsonPropertyName("data")]
    public List<StatsOfDay> Data { get; set; }
}

public class StatsOfDay
{
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
    public int SpamPercent { get; set; }

    [JsonPropertyName("date")]
    public string Date { get; set; }

    [JsonPropertyName("reputation")]
    public double Reputation { get; set; }

    [JsonPropertyName("complaints")]
    public int Complaints { get; set; }

    [JsonPropertyName("deleted_unread")]
    public int DeletedUnread { get; set; }

    [JsonPropertyName("probably_spam")]
    public int ProbablySpam { get; set; }

    [JsonPropertyName("trend")]
    public double Trend { get; set; }

    [JsonPropertyName("probably_spam_percent")]
    public double ProbablySpamPercent { get; set; }
}