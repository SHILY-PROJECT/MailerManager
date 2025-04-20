using System.ComponentModel.DataAnnotations;

namespace MailerManager.Core.Services.MailWizz;

public class MailWizzOptions
{
    [Required] public string AuthPageUrl { get; set; } = string.Empty;
    [Required] public string Login { get; set; } = null!;
    [Required] public string Password { get; set; } = null!;
}