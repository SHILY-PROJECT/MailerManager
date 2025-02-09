using FluentResults;
using MailerManager.Core.Common.DependencyInjection;
using Microsoft.Playwright;

namespace MailerManager.Infrastructure.Services.MailWizz;

public interface IMailWizzAuthentication : ITransientDependency
{
    Task<Result> AuthIfNeed(IBrowser browser);
}