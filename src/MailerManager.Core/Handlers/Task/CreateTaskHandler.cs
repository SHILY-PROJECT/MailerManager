using MediatR;

namespace MailerManager.Core.Handlers.Task;

public class CreateTaskHandler : IRequestHandler<CreateTaskCommand, CreateTaskResult>
{
    public async Task<CreateTaskResult> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        return new CreateTaskResult { SyncId = request.SyncId, IsSuccess = true, Message = "Задание создано" };
    }
}