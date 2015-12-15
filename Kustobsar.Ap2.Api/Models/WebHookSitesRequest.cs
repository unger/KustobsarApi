using Newtonsoft.Json;

namespace Kustobsar.Ap2.Api.Models
{
    public class WebHookSitesRequest
    {
        public bool master { get; set; }
        public string installationId { get; set; }

        [JsonProperty(PropertyName = "params")]
        public WebHookSitesParams data { get; set; }
        public string functionName { get; set; }
    }
}