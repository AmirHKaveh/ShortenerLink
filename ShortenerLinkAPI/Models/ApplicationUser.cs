using Microsoft.AspNetCore.Identity;

namespace ShortLinkGenerator.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public string SecurityCode { get; set; }
        public DateTime? SecurityCodeExpire { get; set; }

        public virtual List<Link> Links { get; set; }
    }
}
