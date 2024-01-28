using Newtonsoft.Json;

namespace CozyCloudBot.DTO;

public class CurrentDTO
{
    [JsonProperty("last_updated")]
    public DateTime LastUpdated;
    
    [JsonProperty("temp_c")]
    public float TempC;

    [JsonProperty("feelslike_c")]
    public float TempFeelLikeC;
}