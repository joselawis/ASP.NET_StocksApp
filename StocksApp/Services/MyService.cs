namespace StocksApp.Services;

public class MyService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public MyService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task Method()
    {
        using (var httpClient = _httpClientFactory.CreateClient())
        {
            var httpRequestMessage = new HttpRequestMessage
            {
                RequestUri = new Uri("url"),
                Method = HttpMethod.Get
            };

            var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
        }
    }
}