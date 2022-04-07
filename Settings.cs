using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YandexDynDnsClient
{
    internal class SettingsData
    { 
        public string LastIp { get; set; } = string.Empty;        
        public string Domain { get; set; } = string.Empty;
        public string Subdomain { get; set; } = string.Empty;
        public int Ttl { get; set; } = 300;
        public int RecordId { get; set; }
        public string Token { get; set; } = string.Empty;
        public string CheckExternalIpUrl { get; set; } = "http://api.ipify.org";
    }
}
