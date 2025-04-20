namespace MailerManager.Core.Services.MailRu;

public interface IMailRuAccessTokenManagerService : IService
{
    Task<IMailRuAccessToken> GetAccessTokenAsync();
    Task<IMailRuAccessToken> RefreshAccessTokenAsync();
}