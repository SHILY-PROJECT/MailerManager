using MailerManager.ConsoleApp;
using MailerManager.Core.Services.MailerManager;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = Host.CreateDefaultBuilder().ConfigureServices(s => s.AddConsoleApp()).Build();

var mailerManager = host.Services.GetRequiredService<IMailerManagerService>();
var result = await mailerManager.RunAsync();