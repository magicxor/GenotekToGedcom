using Newtonsoft.Json;

namespace GenotekToGedcom.Models.Genotek;

public class Relative
{
    [JsonProperty("id")]
    public string? Id { get; set; }

    [JsonProperty("relationType")]
    public string? RelationType { get; set; }
}
