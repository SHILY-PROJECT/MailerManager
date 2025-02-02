using FluentResults;
using MailerManager.Core.Services.MailRuManager;

namespace MailerManager.Core.Clients.MailRu;

public interface IMailRuClient : IClient
{
    Task<Result<string>> RefreshAccessTokenAsync(string refreshToken);
}