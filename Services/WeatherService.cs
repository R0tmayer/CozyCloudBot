using CozyCloudBot.DTO;
using CozyCloudBot.Extensions;
using Newtonsoft.Json;

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
        var weatherDto = JsonConvert.DeserializeObject<WeatherDTO>(jsonResponce);
        string text = BuildResponceText(weatherDto);
        return text;
    }

    private string BuildUrl(string city)
    {
        return "http://api.weatherapi.com/v1/current.json?"
               + $"key={_weatherApiKey}"
               + $"&q={city}"
               + $"&aqi=no";
    }

    private string BuildResponceText(WeatherDTO weatherDto)
    {
        LocationDTO locationDTO = weatherDto.Location;
        CurrentDTO currentDTO = weatherDto.Current;
        
        string location = $"{locationDTO.Country}, {locationDTO.Region}, {locationDTO.City}";
        
        return $"Город: {location}\n"
               + $"Время прогноза: {currentDTO.LastUpdated.ToString("dd/MM/yyyy HH:mm")}\n"
               + $"Температура: {currentDTO.TempC} C°\n"
               + $"Ощущается как: {currentDTO.TempFeelLikeC} C°";
    }
}