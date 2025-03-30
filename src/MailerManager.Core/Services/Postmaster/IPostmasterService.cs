using FluentResults;
using MailerManager.Core.Tools.DependencyInjection;

namespace MailerManager.Core.Services.Postmaster;

public interface IPostmasterService : IScopedDependency
{
    Task<Result> RunAsync();
}