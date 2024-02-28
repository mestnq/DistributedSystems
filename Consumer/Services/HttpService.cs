using System.Net;

namespace Consumer.Services;

public class HttpService: IHttpService
{
    private static HttpClient _sharedClient;

    public HttpService()
    {
        _sharedClient = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:8000/"),
        };
    }

    public async Task<HttpStatusCode> GetHttpStatus(string url)
    {
        HttpStatusCode statusCode = default(HttpStatusCode);
        
        var webRequest = WebRequest.Create(url);
        webRequest.Method = "HEAD"; // Используем метод HEAD для проверки только заголовков

        HttpWebResponse webResponse = (HttpWebResponse)await webRequest.GetResponseAsync();
        var code = webResponse.StatusCode;
        Console.WriteLine(code);
        
        return code;
    }
    
    public async Task UpdateHttpStatusLink(string url)
    {
        url = "71e5c764-3b4d-4576-b624-ebe5e8de3e53"; //TODO DELETE
        var statusCode = await GetHttpStatus(url);
        var request = new HttpRequestMessage(HttpMethod.Patch, _sharedClient + $"Links/refresh-http-status?url={url}&httpStatus={statusCode}");
        Console.WriteLine($"request.RequestUri {request.RequestUri}");

        // request.Content = new StringContent(
        //     JsonConvert.SerializeObject(statusUpdateRequest), Encoding.UTF8, "application/json"
        // );

        var response = await _sharedClient.SendAsync(request);

        Console.WriteLine($"response.IsSuccessStatusCode {response.IsSuccessStatusCode}");
    }
}