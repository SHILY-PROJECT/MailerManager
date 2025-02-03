namespace MailerManager.Infrastructure.Services.Postmaster.Exceptions;

[Serializable]
public class AccessTokenRefreshRequiredException : Exception
{
    private const string DefaultMessage = "Access token refresh required";
    
    public AccessTokenRefreshRequiredException() : base(DefaultMessage) { }
    public AccessTokenRefreshRequiredException(string message) : base(message) { }
    public AccessTokenRefreshRequiredException(string message, Exception innerException) : base(message, innerException) { }
}