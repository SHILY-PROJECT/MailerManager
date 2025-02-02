using MailerManager.Core.Common.DependencyInjection;

namespace MailerManager.Core.Services.MailRuManager;

public interface IMailRuAccessToken
{
    string AccessToken { get; }
}