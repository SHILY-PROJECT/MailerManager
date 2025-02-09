using FluentResults;

namespace MailerManager.Core.Services.MailWizz;

public interface IMailWizzService : IService
{
    Task<Result> RunAsync();
}