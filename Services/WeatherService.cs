using CozyCloudBot.Extensions;

namespace CozyCloudBot.Services;

public class WeatherService : IWeatherService
{
    private readonly string _weatherApiKey;
    private readonly HttpClient _httpClient;

    public WeatherService(string weatherApiKey)
    {
        _weatherApiKey = weatherApiKey;
        _httpClient = new HttpClient();
    }
    
    public async Task<string> GetWeatherAsync(string city)
    {
        string url = BuildUrl(city);
        using HttpResponseMessage responce = await _httpClient.GetAsync(url);
        responce.EnsureSuccessStatusCode().WriteRequestToConsole();

        var jsonResponce = await responce.Content.ReadAsStringAsync();
        return jsonResponce;
    }

    private string BuildUrl(string city)
    {
        return "http://api.weatherapi.com/v1/current.json?"
               + $"key={_weatherApiKey}"
               + $"&q={city}"
               + $"&aqi=no";
    }
}