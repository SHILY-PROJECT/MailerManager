namespace MailerManager.Infrastructure.Services.Postmaster;

public class QueryParameterOptions
{
    public string? Domain { get; set; }
    public DateTime? DateFrom { get; set; }
    public DateTime? DateTo { get; set; }
    public string? MsgType { get; set; }
}