using Newtonsoft.Json;

namespace GenotekToGedcom.Models.Genotek;

public class Node
{
    [JsonProperty("id")]
    public string? Id { get; set; }

    [JsonProperty("type")]
    public string? Type { get; set; }

    [JsonProperty("relationName")]
    public string? RelationName { get; set; }

    [JsonProperty("card", NullValueHandling = NullValueHandling.Ignore)]
    public Card? Card { get; set; }

    [JsonProperty("relationNameNew", NullValueHandling = NullValueHandling.Ignore)]
    public string? RelationNameNew { get; set; }

    [JsonProperty("mother_exists", NullValueHandling = NullValueHandling.Ignore)]
    public bool? MotherExists { get; set; }

    [JsonProperty("father_exists", NullValueHandling = NullValueHandling.Ignore)]
    public bool? FatherExists { get; set; }

    [JsonProperty("can_be_extended", NullValueHandling = NullValueHandling.Ignore)]
    public bool? CanBeExtended { get; set; }

    [JsonProperty("cards_left_behind", NullValueHandling = NullValueHandling.Ignore)]
    public long? CardsLeftBehind { get; set; }
}
