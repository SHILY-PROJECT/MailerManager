namespace MailerManager.Infrastructure.Services.Postmaster.Exceptions;

[Serializable]
public class PostmasterResponseException : Exception
{
    public PostmasterResponseException() { }
    public PostmasterResponseException(string message) : base(message) { }
    public PostmasterResponseException(string message, Exception innerException) : base(message, innerException) { }
}