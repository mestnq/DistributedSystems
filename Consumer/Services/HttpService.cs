using System.Net;

namespace Consumer.Services;

public class HttpService : IHttpService
{
    private static HttpClient _sharedClient;

    public HttpService()
    {
        _sharedClient = new HttpClient();
    }

    public async Task<HttpStatusCode> GetHttpStatus(string url)
    {
        using HttpClient client = new HttpClient();

        HttpResponseMessage response = await client.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine(response.StatusCode.ToString());
        }
        else
        {
            // problems handling here
            Console.WriteLine("Error occurred, the status code is: {0}", response.StatusCode
           );
        }

        return response.StatusCode;
    }

    public async Task UpdateHttpStatusLink(string url)
    {
        var statusCode = Convert.ToInt32(GetHttpStatus(url).Result);
        var uri = $"http://distributedsystems-app-1/Links/refresh-http-status?url={url}&httpStatus={statusCode}";
        // определяем данные запроса
        using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, uri);
        // выполняем запрос
        var responseMessage = await _sharedClient.SendAsync(request);
        
        Console.WriteLine($"response.IsSuccessStatusCode {responseMessage.IsSuccessStatusCode}");
    }
}