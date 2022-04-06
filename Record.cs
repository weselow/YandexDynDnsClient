using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace YandexDynDnsClient
{
    internal class Record
    {
        [JsonPropertyName("content")] public string Content { get; set; } = string.Empty;
        [JsonPropertyName("subdomain")] public string Subdomain { get; set; } = string.Empty;
        [JsonPropertyName("priority")] public string Priority { get; set; } = string.Empty;
        [JsonPropertyName("ttl")] public int Ttl { get; set; }
        [JsonPropertyName("domain")] public string Domain { get; set; } = string.Empty;
        [JsonPropertyName("record_id")] public int RecordId { get; set; }
        [JsonPropertyName("fqdn")] public string Fqdn { get; set; } = string.Empty;
        [JsonPropertyName("type")] public string Type { get; set; } = string.Empty;
    }
}
