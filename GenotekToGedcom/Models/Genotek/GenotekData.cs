﻿// Generated by https://quicktype.io

using Newtonsoft.Json;

namespace GenotekToGedcom.Models.Genotek
{
    public class GenotekData
    {
        [JsonProperty("status")]
        public string? Status { get; set; }

        [JsonProperty("data")]
        public Data? Data { get; set; }
    }
}