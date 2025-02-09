using System.ComponentModel.DataAnnotations;

namespace MailerManager.Core.Services.MailerManager;

public class PlaywrightOptions
{
    [Required]
    public string ExecutablePath { get; set; } = null!;
    
    [Required]
    public bool Headless { get; set; }
}