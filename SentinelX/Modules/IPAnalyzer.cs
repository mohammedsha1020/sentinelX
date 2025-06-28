using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace SentinelX.Modules
{
    public class IPAnalyzer
    {
        private static readonly HttpClient client = new HttpClient();

        public async Task<IPInfo> LookupIPAsync(string ip)
        {
            var url = $"http://ip-api.com/json/{ip}";
            var response = await client.GetStringAsync(url);
            var json = JObject.Parse(response);
            return new IPInfo
            {
                IP = ip,
                Country = json["country"]?.ToString(),
                Org = json["org"]?.ToString(),
                ISP = json["isp"]?.ToString(),
                City = json["city"]?.ToString(),
                ThreatLevel = json["status"]?.ToString() == "fail" ? "Unknown" : "Normal"
            };
        }
    }

    public class IPInfo
    {
        public string IP { get; set; }
        public string Country { get; set; }
        public string Org { get; set; }
        public string ISP { get; set; }
        public string City { get; set; }
        public string ThreatLevel { get; set; }
    }
} 