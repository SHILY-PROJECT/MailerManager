using System.Diagnostics;
using System.Text.Encodings.Web;
using System.Text.Json;
using MailerManager.Core.Handlers;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MailerManager.Core.Behaviors.Global;

public class GlobalLoggingBehavior<TRequest, TResponse>(ILogger<GlobalLoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : BaseCommandRequest
    where TResponse : BaseCommandResult
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var logInfo = $"SyncId={request.SyncId}. Method={GetType().Name}.{nameof(Handle)}. RequestType={typeof(TResponse).Name} ";
        
        logger.LogInformation($"{logInfo}Started operation. Request={JsonSerializer.Serialize(request)}");

        var sw = Stopwatch.StartNew();

        var response = await next();
        response.SyncId = request.SyncId;

        sw.Stop();

        logger.LogInformation($"{logInfo}Finished operation. Response={JsonSerializer.Serialize(response, new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping })}");
        
        return response;
    }
}