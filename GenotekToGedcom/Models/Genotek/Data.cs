using Newtonsoft.Json;

namespace GenotekToGedcom.Models.Genotek;

public class Data
{
    [JsonProperty("edges")]
    public Edge[]? Edges { get; set; }

    [JsonProperty("nodes")]
    public Node[]? Nodes { get; set; }

    [JsonProperty("treeId")]
    public string? TreeId { get; set; }

    [JsonProperty("status")]
    public string? Status { get; set; }

    [JsonProperty("cards_count")]
    public CardsCount? CardsCount { get; set; }
}
