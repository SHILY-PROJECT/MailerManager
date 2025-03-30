using System.Reflection;
using MailerManager.ConsoleApp;
using MailerManager.Core.Services.MailerManager;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var assembly =  Assembly.GetExecutingAssembly();

var host = Host.CreateDefaultBuilder().ConfigureServices(s => s.AddConsoleApp(assembly)).Build();

await using var scope = host.Services.CreateAsyncScope();

var mailerManager = scope.ServiceProvider.GetRequiredService<IMailerManagerService>();
var result = await mailerManager.RunAsync();



