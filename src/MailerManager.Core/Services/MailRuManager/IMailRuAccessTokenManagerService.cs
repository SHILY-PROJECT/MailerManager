namespace MailerManager.Core.Services.MailRuManager;

public interface IMailRuAccessTokenManagerService : IService
{
    Task<IMailRuAccessToken> GetAccessTokenAsync();
    Task<IMailRuAccessToken> RefreshAccessTokenAsync();
}