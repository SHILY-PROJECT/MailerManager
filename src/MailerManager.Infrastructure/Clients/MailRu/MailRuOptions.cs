using System.ComponentModel.DataAnnotations;

namespace MailerManager.Infrastructure.Clients.MailRu;

public class MailRuOptions
{
    [Required]
    public string AuthUrl { get; set; }
}