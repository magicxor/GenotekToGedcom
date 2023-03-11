using Newtonsoft.Json;

namespace GenotekToGedcom.Models.Genotek;

public class CardsCount
{
    [JsonProperty("full_tree")]
    public long FullTree { get; set; }

    [JsonProperty("relatives_only")]
    public long RelativesOnly { get; set; }

    [JsonProperty("generations_only")]
    public long GenerationsOnly { get; set; }
}