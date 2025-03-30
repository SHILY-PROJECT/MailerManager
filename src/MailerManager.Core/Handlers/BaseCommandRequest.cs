using System.ComponentModel.DataAnnotations;

namespace MailerManager.Core.Handlers;

public class BaseCommandRequest
{
    [Required]
    public Guid SyncId { get; set; }
}