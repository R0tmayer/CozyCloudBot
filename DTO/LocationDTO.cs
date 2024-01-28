using Newtonsoft.Json;

namespace CozyCloudBot.DTO;

public class LocationDTO
{
    [JsonProperty("name")]
    public string City;
    
    [JsonProperty("region")]
    public string Region;
    
    [JsonProperty("country")]
    public string Country;

}