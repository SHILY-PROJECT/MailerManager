namespace MailerManager.Core.Services.MailRuManager;

public class MailRuAccessToken : IMailRuAccessToken
{
    public MailRuAccessToken() { }
    public MailRuAccessToken(string accessToken) => AccessToken = accessToken;
    public string AccessToken { get; init; }
}