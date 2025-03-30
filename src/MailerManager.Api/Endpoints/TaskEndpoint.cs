using MailerManager.Api.Endpoints.Scanner;
using MailerManager.Core.Handlers.Task;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MailerManager.Api.Endpoints;

public class TaskEndpoint : IEndpoint
{
    public void AddEndpoints(WebApplication app)
    {
        var appCroup = app.MapGroup("/api/tasks");

        appCroup
            .MapPost("create-task", async (IMediator mediator, [FromBody] CreateTaskCommand request) => await mediator.Send(request))
            .WithName("create-task")
            .WithSummary("Создать новую задачу")
            .WithOpenApi();
    }
}