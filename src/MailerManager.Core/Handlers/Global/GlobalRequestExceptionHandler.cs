using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace MailerManager.Core.Handlers.Global;

public class GlobalRequestExceptionHandler<TRequest, TResponse, TException>(ILogger<GlobalRequestExceptionHandler<TRequest, TResponse, TException>> logger)
    : IRequestExceptionHandler<TRequest, TResponse, TException>
    where TRequest : BaseCommandRequest, new()
    where TResponse : BaseCommandResult, new()
    where TException : Exception
{
    public Task Handle(TRequest request, TException exception, RequestExceptionHandlerState<TResponse> state, CancellationToken cancellationToken)
    {
        logger.LogWarning(exception, $"$SyncId={request.SyncId} Method={GetType().Name}.{nameof(Handle)} Error. RequestType: {typeof(TRequest).Name}");
        
        state.SetHandled(new TResponse
        {
            SyncId = request.SyncId,
            IsSuccess = false,
            Message = exception.Message
        });
        
        return Task.CompletedTask;
    }
}