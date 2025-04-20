using FluentResults;
using MailerManager.Core.Context;

namespace MailerManager.Core.Actions;

public interface IAction
{
    Task<Result> ExecuteAsync(MailerManagerContext context);
}