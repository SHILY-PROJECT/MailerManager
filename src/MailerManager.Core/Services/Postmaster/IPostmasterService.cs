using FluentResults;
using MailerManager.Core.Common.DependencyInjection;

namespace MailerManager.Core.Services.Postmaster;

public interface IPostmasterService : IScopedDependency
{
    Task<Result> RunAsync();
}