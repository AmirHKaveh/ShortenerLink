namespace ShortLinkGenerator.Helpers
{
    public static class Extensions
    {
        public static string GenerateSecurityCode(int lenght = 5)
        {
            bool flag = true;
            Random random = new Random();
            return new string((from s in Enumerable.Repeat("0123456789", lenght)
                               select s[random.Next(s.Length)]).ToArray());
        }
        public static string GenerateLinkShortKey(int length = 5)
        {
            string key = Guid.NewGuid().ToString().Replace("-", "").Substring(0, length);
            return key;
        }
    }
}
