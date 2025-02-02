using MailerManager.Core.Services.MailRuManager;
using MailerManager.Core.Services.Postmaster;
using Microsoft.Extensions.Options;

namespace MailerManager.Infrastructure.Services.Postmaster;

public class PostmasterContext(IOptions<PostmasterOptions> options) : IPostmasterContext
{
    public IMailRuAccessToken Token { get; set; } = null!;
    public PostmasterOptions Options { get; set; } = options.Value;
}