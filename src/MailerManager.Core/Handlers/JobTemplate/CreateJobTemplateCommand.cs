using MediatR;

namespace MailerManager.Core.Handlers.JobTemplate;

public class CreateJobTemplateCommand : BaseCommandRequest, IRequest<CreateJobTemplateResult>
{
    public Guid JobTemplateId { get; set; }
}