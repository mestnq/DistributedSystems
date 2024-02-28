using System.Net;

namespace Consumer.Services;

public interface IHttpService
{
    public Task<HttpStatusCode> GetHttpStatus(string url);
    public Task UpdateHttpStatusLink(string url);
}