using MediatR;

namespace MailerManager.Core.Handlers.Task;

public class CreateTaskCommand : BaseCommandRequest, IRequest<CreateTaskResult>
{
    
}