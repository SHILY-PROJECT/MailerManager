using MediatR;

namespace MailerManager.Core.Handlers.JobTemplate;

public class CreateJobTemplateHandler : IRequestHandler<CreateJobTemplateCommand, CreateJobTemplateResult>
{
    public async Task<CreateJobTemplateResult> Handle(CreateJobTemplateCommand request, CancellationToken cancellationToken)
    {
        return new CreateJobTemplateResult { SyncId = request.SyncId, IsSuccess = true, Message = "Задание создано" };
    }
}