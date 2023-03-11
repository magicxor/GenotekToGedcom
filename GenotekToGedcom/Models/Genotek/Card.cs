using Newtonsoft.Json;

namespace GenotekToGedcom.Models.Genotek;

public class Card
{
    [JsonProperty("id")]
    public string? Id { get; set; }

    [JsonProperty("gender")]
    public string? Gender { get; set; }

    [JsonProperty("patientId")]
    public long? PatientId { get; set; }

    [JsonProperty("email")]
    public string? Email { get; set; }

    [JsonProperty("liveOrDead")]
    public long? LiveOrDead { get; set; }

    [JsonProperty("name")]
    public string[]? Name { get; set; }

    [JsonProperty("surname")]
    public string[]? Surname { get; set; }

    [JsonProperty("middleName")]
    public string[]? MiddleName { get; set; }

    [JsonProperty("maidenName")]
    public string[]? MaidenName { get; set; }

    [JsonProperty("phone")]
    public string[]? Phone { get; set; }

    [JsonProperty("ethnicityId")]
    public long[]? EthnicityId { get; set; }

    [JsonProperty("ethnicity")]
    public string[]? Ethnicity { get; set; }

    [JsonProperty("birthdate")]
    public Birthdate[]? Birthdate { get; set; }

    [JsonProperty("deathdate")]
    public Birthdate[]? Deathdate { get; set; }

    [JsonProperty("birthplace")]
    public string[]? Birthplace { get; set; }

    [JsonProperty("birthplaceParsed")]
    public Dictionary<string, string>[]? BirthplaceParsed { get; set; }

    [JsonProperty("deathplace")]
    public string[]? Deathplace { get; set; }

    [JsonProperty("deathplaceParsed")]
    public Dictionary<string, string>[]? DeathplaceParsed { get; set; }

    [JsonProperty("relatives")]
    public Relative[]? Relatives { get; set; }

    [JsonProperty("relationships")]
    public Relationship[]? Relationships { get; set; }

    [JsonProperty("type")]
    public string? Type { get; set; }
}
