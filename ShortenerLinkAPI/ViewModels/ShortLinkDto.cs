namespace ShortLinkGenerator.ViewModels
{
    public class ShortLinkRequestDto
    {
        public string Url { get; set; }
    }
    public class ShortLinkResponseDto
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public string Url { get; set; }
    }
}
