using FluentResults;

namespace MailerManager.Core.Services.MailerManager;

public interface IMailerManagerService : IService
{
    Task<Result> RunAsync();
}