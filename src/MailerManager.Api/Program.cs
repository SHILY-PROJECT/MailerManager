using System.Reflection;
using MailerManager.Api;
using MailerManager.Core;
using MailerManager.Infrastructure;
using Scalar.AspNetCore;

var assembly = Assembly.GetExecutingAssembly();
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddApi(assembly);
builder.Services.AddCore();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.MapOpenApi();
app.MapScalarApiReference(opt => opt.WithTitle(typeof(Program).Namespace!).WithTheme(ScalarTheme.Kepler).WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.RestSharp));
app.UseHttpsRedirection();
app.AddEndpoints();

await app.RunAsync();