using FluentResults;
using MailerManager.Core.Tools.DependencyInjection;
using Microsoft.Playwright;

namespace MailerManager.Infrastructure.Services.MailWizz;

public interface IMailWizzAuthentication : ITransientDependency
{
    Task<Result> AuthIfNeed(IBrowser browser);
}