using Newtonsoft.Json;

namespace GenotekToGedcom.Models.Genotek;

public class Edge
{
    [JsonProperty("from")]
    public string? From { get; set; }

    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("to")]
    public string? To { get; set; }
}
