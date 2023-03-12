using Newtonsoft.Json;

namespace GenotekToGedcom.Models.Genotek;

public class Birthdate
{
    [JsonProperty("day")]
    public int? Day { get; set; }

    [JsonProperty("month")]
    public int? Month { get; set; }

    [JsonProperty("year")]
    public int? Year { get; set; }
}