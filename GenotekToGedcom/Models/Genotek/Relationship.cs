using Newtonsoft.Json;

namespace GenotekToGedcom.Models.Genotek;

public class Relationship
{
    [JsonProperty("with")]
    public string? With { get; set; }

    [JsonProperty("type")]
    public string? Type { get; set; }

    [JsonProperty("finished")]
    public long? Finished { get; set; }

    [JsonProperty("from")]
    public Birthdate[]? From { get; set; }

    [JsonProperty("to")]
    public Birthdate[]? To { get; set; }
}
