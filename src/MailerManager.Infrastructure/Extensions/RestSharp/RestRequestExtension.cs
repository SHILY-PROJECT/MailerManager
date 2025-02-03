using RestSharp;

namespace MailerManager.Infrastructure.Extensions.RestSharp;

public static class RestRequestExtension
{
    public static RestRequest AddQueryParameterWhereValueNotNull(this RestRequest request, string name, string? value) =>
        string.IsNullOrWhiteSpace(value) ? request : request.AddQueryParameter(name, value);
}