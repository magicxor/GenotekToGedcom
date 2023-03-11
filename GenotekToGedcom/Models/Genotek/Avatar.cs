using Newtonsoft.Json;

namespace GenotekToGedcom.Models.Genotek;

public class Avatar
{
    [JsonProperty("id")]
    public string? Id { get; set; }

    [JsonProperty("fileId")]
    public string? FileId { get; set; }

    [JsonProperty("original")]
    public string? Original { get; set; }

    [JsonProperty("bw")]
    public string? Bw { get; set; }

    [JsonProperty("improved")]
    public string? Improved { get; set; }
}
