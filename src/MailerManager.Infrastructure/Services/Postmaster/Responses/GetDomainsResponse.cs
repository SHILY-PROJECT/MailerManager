using System.Text.Json.Serialization;

namespace MailerManager.Infrastructure.Services.Postmaster.Responses;

public class GetDomainsResponse
{
    [JsonPropertyName("ok")]
    public bool Ok { get; set; }

    [JsonPropertyName("domains")]
    public List<DomainModel> Domains { get; set; }
}

public class DomainModel
{
    [JsonPropertyName("domain")]
    public string Domain { get; set; }
}
