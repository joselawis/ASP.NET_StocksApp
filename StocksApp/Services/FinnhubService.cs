using System.Text.Json;
using StocksApp.ServiceContracts;

namespace StocksApp.Services;

public class FinnhubService : IFinnhubService
{
    private readonly IConfiguration _configuration;
    private readonly IHttpClientFactory _httpClientFactory;

    public FinnhubService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    public async Task<Dictionary<string, object>> GetStockPriceQuote(string stockSymbol)
    {
        using (var httpClient = _httpClientFactory.CreateClient())
        {
            var httpRequestMessage = new HttpRequestMessage
            {
                RequestUri =
                    new Uri(
                        $"https://finnhub.io/api/v1/quote?symbol={stockSymbol}&token={_configuration["FinnhubToken"]}"),
                Method = HttpMethod.Get
            };

            var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

            var stream = httpResponseMessage.Content.ReadAsStream();
            var streamReader = new StreamReader(stream);

            var response = streamReader.ReadToEnd();
            var responseDictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(response);

            if (responseDictionary == null) throw new InvalidOperationException("No response from finnhub server");

            if (responseDictionary.ContainsKey("error")) throw new InvalidOperationException("Error");

            return responseDictionary;
        }
    }
}