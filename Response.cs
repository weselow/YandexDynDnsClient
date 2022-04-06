using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace YandexDynDnsClient
{
    internal class Response
    {
        [JsonPropertyName("domain")] public string Domain { get; set; } = string.Empty;
        [JsonPropertyName("record")] public Record Record { get; set; } = new();
        [JsonPropertyName("success")] public string Success { get; set; } = string.Empty;
        [JsonPropertyName("error")] public string Error { get; set; } = string.Empty;
        [JsonExtensionData] public Dictionary<string, JsonElement>? ExtensionData { get; set; }

    }
}
