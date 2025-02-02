namespace MailerManager.Infrastructure.Services.Postmaster.Exceptions;

[Serializable]
public class AccessTokenRefreshRequiredException : Exception
{
    public AccessTokenRefreshRequiredException() { }
    public AccessTokenRefreshRequiredException(string message) : base(message) { }
    public AccessTokenRefreshRequiredException(string message, Exception innerException) : base(message, innerException) { }
}