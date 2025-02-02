using MailerManager.Core.Common.DependencyInjection;
using MailerManager.Core.Services.MailRuManager;
using Microsoft.Extensions.Options;

namespace MailerManager.Infrastructure.Services.Postmaster;

public class PostmasterContext(IOptions<PostmasterOptions> options) : IScopedDependency
{
    public IMailRuAccessToken Token { get; set; } = null!;
    public PostmasterOptions Options { get; set; } = options.Value;
}