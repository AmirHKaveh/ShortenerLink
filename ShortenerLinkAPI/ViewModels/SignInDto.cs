using System.ComponentModel.DataAnnotations;

namespace ShortLinkGenerator.ViewModels
{
    public class SignInDto
    {
        [Display(Name = "تلفن همراه")]
        [Required(ErrorMessage = "لطفا {0} خود را وارد نمایید")]
        [RegularExpression(@"^[0][9]?(9\d{9})$", ErrorMessage = "{0} معتبر نمی باشد")]
        public string Mobile { get; set; }
    }

    public class VerifyDto
    {
        [Display(Name = "تلفن همراه")]
        [Required(ErrorMessage = "لطفا {0} خود را وارد نمایید")]
        [RegularExpression(@"^[0][9]?(9\d{9})$", ErrorMessage = "{0} معتبر نمی باشد")]
        public string Mobile { get; set; }
        public string Code { get; set; }
    }
}
