using MailerManager.Core.Common.DependencyInjection;
using MailerManager.Core.Services.MailRuManager;

namespace MailerManager.Core.Services.Postmaster;

public interface IPostmasterContext : IScopedDependency
{
    IMailRuAccessToken Token { get; set; }
    PostmasterOptions Options { get; set; }
}