using MailerManager.ConsoleApp;
using MailerManager.Core.Services.MailerManager;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = Host.CreateDefaultBuilder().ConfigureServices(s => s.AddConsoleApp()).Build();

await using var scope = host.Services.CreateAsyncScope();
var mailerManager = scope.ServiceProvider.GetRequiredService<IMailerManagerService>();
var result = await mailerManager.RunAsync();