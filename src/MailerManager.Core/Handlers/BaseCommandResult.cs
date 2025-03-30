namespace MailerManager.Core.Handlers;

public class BaseCommandResult
{
    public Guid SyncId { get; set; }
    public bool IsSuccess { get; set; }
    public string? Message { get; set; }
}