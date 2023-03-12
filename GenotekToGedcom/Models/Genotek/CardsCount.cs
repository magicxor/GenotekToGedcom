using Newtonsoft.Json;

namespace GenotekToGedcom.Models.Genotek;

public class CardsCount
{
    [JsonProperty("full_tree")]
    public int FullTree { get; set; }

    [JsonProperty("relatives_only")]
    public int RelativesOnly { get; set; }

    [JsonProperty("generations_only")]
    public int GenerationsOnly { get; set; }
}