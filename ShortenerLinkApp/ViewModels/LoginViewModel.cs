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

    public class OTPViewModel
    {
        [Display(Name = "Mobile")]
        [Required(ErrorMessage = "Please Enter {0}")]
        [RegularExpression(@"^[0][9]?(9\d{9})$", ErrorMessage = "{0} Invalid")]
        public string Mobile { get; set; }
        public string Code { get; set; }
    }
    public class OTPSharedDataService
    {
        public string MobileNumber { get; set; }
    }
    public class LoginResponse
    {
        public string token { get; set; }
    }
}
