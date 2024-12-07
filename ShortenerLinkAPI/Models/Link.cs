using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShortLinkGenerator.Models
{
    public class Link
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(450)]
        public string OriginalUrl { get; set; }
        [MaxLength(450)]
        public string Url { get; set; }
        [Required]
        [MaxLength(10)]
        public string Key { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        public DateTime CreateDate { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
