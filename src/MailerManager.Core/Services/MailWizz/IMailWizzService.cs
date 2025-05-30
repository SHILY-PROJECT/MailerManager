﻿using FluentResults;
using MailerManager.Core.Context;

namespace MailerManager.Core.Services.MailWizz;

public interface IMailWizzService : IService
{
    Task<Result> ExecuteAsync(MailerManagerContext context);
}