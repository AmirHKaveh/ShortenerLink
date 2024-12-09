using System.ComponentModel.DataAnnotations;

namespace ShortenerLinkApp.ViewModels
{
    public class LoginViewModel
    {
        [Display(Name = "Mobile")]
        [Required(ErrorMessage = "Please Enter {0}")]
        [RegularExpression(@"^[0][9]?(9\d{9})$", ErrorMessage = "{0} Invalid")]
        public string Mobile { get; set; }
    }
}
