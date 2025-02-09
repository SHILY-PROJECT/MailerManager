using FluentResults;

namespace MailerManager.Core.Services;

public interface IAction
{
    Task<Result> ExecuteAsync();
}