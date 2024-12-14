using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ShortenerLinkApp.ViewModels
{
    public class ShortLinkRequestViewModel
    {
        [DisplayName("Url")]
        [Required(ErrorMessage = "Please Enter Your {0}")]
        [DataType(DataType.Url, ErrorMessage = "Invalid Your {0}")]
        public string Url { get; set; }
    }

    public class ShortLinkResponseViewModel
    {
        public bool status { get; set; }
        public string message { get; set; }
        public string url { get; set; }
    }
}
