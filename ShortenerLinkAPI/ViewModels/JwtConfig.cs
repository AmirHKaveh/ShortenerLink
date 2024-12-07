namespace ShortLinkGenerator.ViewModels
{
    public class JwtConfig
    {
        public string Key { get; set; }
        public int DurationInMinutes { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
}