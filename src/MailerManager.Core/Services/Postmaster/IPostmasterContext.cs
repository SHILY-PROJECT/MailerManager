using MailerManager.Core.Services.MailRu;
using MailerManager.Core.Tools.DependencyInjection;

namespace MailerManager.Core.Services.Postmaster;

public interface IPostmasterContext : IScopedDependency
{
    IMailRuAccessToken Token { get; set; }
    PostmasterOptions Options { get; set; }
}