using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ShortenerLinkApp.ViewModels
{
    public class ShortLinkRequestDto
    {
        [DisplayName("Url")]
        [Required(ErrorMessage = "Please Enter Your {0}")]
        [DataType(DataType.Url, ErrorMessage = "Invalid Your {0}")]
        public string Url { get; set; }
    }
}
