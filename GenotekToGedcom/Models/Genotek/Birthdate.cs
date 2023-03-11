using Newtonsoft.Json;

namespace GenotekToGedcom.Models.Genotek;

public class Birthdate
{
    [JsonProperty("day")]
    public long? Day { get; set; }

    [JsonProperty("month")]
    public long? Month { get; set; }

    [JsonProperty("year")]
    public long? Year { get; set; }
}