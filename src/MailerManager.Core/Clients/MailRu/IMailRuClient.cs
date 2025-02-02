namespace MailerManager.Core.Clients.MailRu;

public interface IMailRuClient : IClient
{
    Task<string> RefreshAccessTokenAsync(string refreshToken);
}