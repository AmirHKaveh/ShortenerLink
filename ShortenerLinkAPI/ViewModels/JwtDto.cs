using System.Text.Json.Serialization;

namespace ShortLinkGenerator.ViewModels
{
    public class JwtDto
    {
        [JsonIgnore]
        public bool Status { get; set; }
        public string Token { get; set; }
        [JsonIgnore]
        public string Message { get; set; }
        public List<string> Roles { get; set; }
    }
}
